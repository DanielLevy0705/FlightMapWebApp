(function ($) {
    $.fn.display1 = function (ctx, x, y) {
        var planeSize = 35;

        var plane = new Image();
        plane.onload = function () {
            ctx.drawImage(plane, x - planeSize / 2,
                y - planeSize / 2, planeSize, planeSize);

        }
        plane.src = "../../../Content/airplane-icon.png";
        plane.alt = "airplane-icon";
    }
})(jQuery);