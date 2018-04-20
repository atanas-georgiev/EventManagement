import { IResource } from "./resource.model";

export interface IEvent {
    id: number,
    additionalInfo: string,
    lecturerName: string,
    name: string,
    price: number
    resourceId: number,
    resource: IResource,
    end: Date,
    start: Date
}