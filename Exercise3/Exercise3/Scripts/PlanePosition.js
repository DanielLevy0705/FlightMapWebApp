var c = document.getElementById("canvas1");
c.width = window.innerWidth;
c.height = window.innerHeight;
var ctx = c.getContext("2d");
var wEdge = 180, hEdge = 90;
var x, y, r1, r2;
class range {
    constructor(start, end) {
        this.start = start;
        this.end = end;
    }
}
r1 = new range(-wEdge, wEdge);
r2 = new range(-hEdge, hEdge);
r3 = new range(0, window.innerWidth);
r4 = new range(0, window.innerHeight);
@{
    var res = Model.model.GetData(Model.Param1,
        int.Parse(Model.Param2), new [] { "Lon", "Lat" });
}
var convert = function (param, range1Start, range1End, range2Start, range2End) {
    return ((param - range1Start) * (range2End - range2Start) /
        (range1End - range1Start) + range2Start);
}
x = convert(@res["Lon"], r1.start, r1.end, r3.start, r3.end);
y = convert(@res["Lat"], r2.start, r2.end, r4.start, r4.end);
ctx.beginPath();
ctx.arc(x, y, 9, 0, 2 * Math.PI);
ctx.fillStyle = "blue";
ctx.fill();
ctx.beginPath();
ctx.arc(x, y, 5, 0, 2 * Math.PI);
ctx.fillStyle = "red";
ctx.fill();