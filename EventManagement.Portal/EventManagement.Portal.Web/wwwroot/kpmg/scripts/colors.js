'use strict';

var KPMGcolors = {
    // Palette selection function colors
    // accepts arguments for color space and single difined or random color from the array 
    palette: function (elem, array, opacity) {
        var colors = [];
        //opacity on rgba space
        opacity = (opacity && 0 < opacity <= 1) ? opacity : 1;
        //assign color space
        colors = (elem == 'hex') ? ['#1B77AF', '#4A4499', '#C6007E', '#470A68', '#00A3A1', '#00338D', '#009A44', '#43B02A', '#EAAA00', '#F68D2E', '#1CA4D3', '#BC204B', '#7C2E87']
                : (elem == 'rgba') ? ['rgba(27,119,175,' + opacity + ')', 'rgba(74,68,153,' + opacity + ')', 'rgba(71,10,104,' + opacity + ')', 'rgba(0,163,161,' + opacity + ')', 'rgba(0,51,141,' + opacity + ')', 'rgba(0,154,68,' + opacity + ')', 'rgba(67,176,42,' + opacity + ')', 'rgba(234,170,0,' + opacity + ')', 'rgba(246,141,46,' + opacity + ')', 'rgba(28,164,211,' + opacity + ')', 'rgba(188,32,75,' + opacity + ')', 'rgba(198,0,126,' + opacity + ')', 'rgba(124,46,135,' + opacity + ')']
                : (elem == 'hsl') ? ['hsl(203, 73%, 40%)', 'hsl(244, 38%, 43%)', 'hsl(322, 100%, 39%)', 'hsl(279, 82%, 22%)', 'hsl(179, 100%, 32%)', 'hsl(218, 100%, 28%)', 'hsl(146, 100%, 30%)', 'hsl(109, 61%, 43%)', 'hsl(44, 100%, 46%)', 'hsl(29, 92%, 57%)', 'hsl(195, 77%, 47%)', 'hsl(343, 71%, 43%)', 'hsl(322, 100%, 39%)', 'hsl(293, 49%, 35%)']
                : 'Incorrect palette definition.';

        // specific or random color, or entire array if second argument is not defined
        if (array && typeof array == 'number') {
            var colorCount = colors.length;
            if (array > colorCount) {
                var temp = array % colorCount;
            } else {
                var temp = array;
            }
            return colors[temp - 1];
        } else if (array == 'random') {
            var usedColors = [];
            var randColor = colors[Math.floor(Math.random() * colors.length)];
            return randColor;
        } else {
            return colors;
        }
    }
};