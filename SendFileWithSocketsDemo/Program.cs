

using System.Net.Sockets;

// اطلاعات مقصد برای اتصال
TcpClient client = new TcpClient("localhost", 8888);

// آدرس فایل
string filePath = "FileName.exe";

// دریافت فایل برای ارسال
FileStream fileStream = File.OpenRead(filePath);

// نام فایل
string fileName = Path.GetFileName(filePath);

// ارسال نام فایل
byte[] fileNameBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
client.GetStream().Write(BitConverter.GetBytes(fileNameBytes.Length), 0, 4);
client.GetStream().Write(fileNameBytes, 0, fileNameBytes.Length);

var fileLength = fileStream.Length;
int i = 1;
// ارسال فایل در بسته های 1 مگابایتی
byte[] buffer = new byte[1048576];
int bytesRead;
while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
{
    var count = Convert.ToInt32(Math.Ceiling(fileLength / 1048576.0));
    var percent = i * 100 / count;
    // نمایش درصد پیشرفت
    Console.WriteLine($" %{percent}");
    i++;
    client.GetStream().Write(buffer, 0, bytesRead);
}

// پایان - بستن کانکشن و فایل
fileStream.Close();
client.Close();