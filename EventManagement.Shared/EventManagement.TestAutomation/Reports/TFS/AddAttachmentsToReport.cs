using EventManagement.TestAutomation.Data;
using EventManagement.TestAutomation.Paths;
using EventManagement.TestAutomation.Reports.TFS.Models;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EventManagement.TestAutomation.Reports.TFS
{
    public static class AddAttachmentsToReport
    {
        public static void AddScreenshots(string tfsURL, string tfsToken, string projectId, string buildDefinitionName, string projectName, string nameOfProjectWithTests)
        {

            var dictionary = DictionaryInteractions.ReadFromPropertiesFile(ReturnPath.SolutionFolderPath() + nameOfProjectWithTests + "/ExtentReport/ReportProperties.txt");
            string[] fileArray = Directory.GetFiles(dictionary["ReportPath"]);
            var onlyScreenshots = fileArray.Where(s => s.IndexOf(".png") != -1);
            if (onlyScreenshots.ToList().Count != 0)
            {
                VssConnection vc = new VssConnection(new Uri(tfsURL), new VssBasicCredential(string.Empty, tfsToken));
                BuildHttpClient bhc = vc.GetClient<BuildHttpClient>();
                ProjectHttpClient ddd = vc.GetClient<ProjectHttpClient>();

                //ITestManagementTeamProject ddd1 = ddd.GetProject("hgfyjh");



                var buildsPerProject = bhc.GetBuildsAsync(projectId).GetAwaiter().GetResult();
                var lastBuildId = buildsPerProject.Where<Build>(x => x.Definition.Name == buildDefinitionName).Max<Build>(z => z.Id);
                Build lastBuild = buildsPerProject.FirstOrDefault<Build>(x => x.Id == lastBuildId);

                Console.Write("Last Build Number: " + lastBuildId);
                TestManagementHttpClient ithc = vc.GetClient<TestManagementHttpClient>();
                QueryModel qm = new QueryModel("Select * From TestRun Where BuildNumber Contains '" + lastBuild.BuildNumber + "'");
                List<TestRun> testruns = ithc.GetTestRunsByQueryAsync(qm, projectName).Result;
                Console.Write("testruns.Count: " + testruns.Count);

                foreach (TestRun testrun in testruns)
                {
                    List<TestCaseResult> testresults = ithc.GetTestResultsAsync(projectName, testrun.Id).Result;
                    Console.Write("testresults.Count: " + testresults.Count);
                    foreach (TestCaseResult tcr in testresults)
                    {
                        var testNamesFromScreents = onlyScreenshots.Select(s => s.ToLower().Split(new string[] { "sec\\" }, StringSplitOptions.None)[1].Split(new string[] { "20" }, StringSplitOptions.None)[0]).Distinct();
                        //var screentsShotPerTest = onlyScreenshots.Where(s => s.Split(new string[] { "sec\\" }, StringSplitOptions.None)[1].Split(new string[] { "20" }, StringSplitOptions.None)[0] == scenarioName.Replace(" ", string.Empty));

                        Console.WriteLine("tcr.Id: " + tcr.Id);
                        if (testNamesFromScreents.ToList().Contains(tcr.AutomatedTestName.Split('.').Last().ToLower()))
                        {
                            Console.WriteLine("recognize Test: " + tcr.AutomatedTestName.Split('.').Last().ToLower());
                            using (var client = new HttpClient())
                            {
                                string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", tfsToken)));
                                client.BaseAddress = new Uri(tfsURL);  //url of our account
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                                var screentsShotPerTest = onlyScreenshots.Where(s => s.Split(new string[] { "sec\\" }, StringSplitOptions.None)[1].Split(new string[] { "20" }, StringSplitOptions.None)[0].ToLower() == tcr.AutomatedTestName.Split('.').Last().ToLower());

                                foreach (var screenshot in screentsShotPerTest)
                                {
                                    Console.WriteLine("screentsShotPerTest: " + screenshot);
                                    var post = new PostImageTfs()
                                    {
                                        Stream = Convert.ToBase64String(File.ReadAllBytes(screenshot)),
                                        AttachmentType = "GeneralAttachment",
                                        Comment = "screenshot of error",
                                        FileName = screenshot.Split(new string[] { "sec\\" }, StringSplitOptions.None)[1]
                                    };
                                    Console.WriteLine("tcr.TestRun.Id: " + tcr.TestRun.Id);
                                    Console.WriteLine("tcr.Id: " + tcr.Id);
                                    Console.WriteLine("screenshot.Split(new string[] { DateTime.Now.Year.ToString() }, StringSplitOptions.None)[2] " + screenshot.Split(new string[] { DateTime.Now.Year.ToString() }, StringSplitOptions.None)[2]);

                                    var test = new StringContent($"{{\"stream\": \"{post.Stream}\",\"fileName\": \"{post.FileName}\",\"comment\": \"{post.Comment}\",\"attachmentType\": \"{post.AttachmentType}\"}}", Encoding.UTF8, "application/json");

                                    HttpResponseMessage response = client.PostAsync("/DefaultCollection/" + projectName + "/_apis/test/runs/" + tcr.TestRun.Id + "/results/" + tcr.Id + "/attachments?api-version=2.0-preview.1", test).Result;
                                    Console.WriteLine("response: " + response.StatusCode);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
