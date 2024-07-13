using QRCodeGenerator;

namespace ConsopleApp;

class Program
{
    static void Main(string[] args)
    {
        var result = new QRCode("https://www.zestien3.nl/furniture/index.html");
        result.Bitmap!.Save("qr.png");
    }
}
