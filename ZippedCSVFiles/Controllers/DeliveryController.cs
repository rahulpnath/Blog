using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace ZippedCSVFiles.Controllers
{
    public class DeliveryJobRecord
    {
        public string StoreName { get; set; }
        public string OrderNo { get; set; }
        public string Driver { get; set; }
        public string VehicleNumber { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
    }

    public class File
    {
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class DeliveryController : ControllerBase
    {
        [HttpGet]
        public IActionResult DownloadDeliveriesForToday()
        {
            var zipFile = GetZippedFile(DateTime.UtcNow);
            return File(zipFile.Bytes, "application/octet-stream", zipFile.FileName);
        }

        private File GetZippedFile(DateTime deliveryDate)
        {
            var deliveries = GetDeliveriesForDate(deliveryDate);

            var csvFiles = deliveries
                .GroupBy(a => a.StoreName)
                .Select(store =>
                    ToCSVFile(store.ToList(), $"{store.Key} {deliveryDate:dd-MM-yyyy} - Delivery.csv"))
                .ToList();

            return ToZip(csvFiles);

        }

        private File ToZip(List<File> files)
        {
            var compressedFileStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var zipEntry = zipArchive.CreateEntry(file.FileName);

                    using (var originalFileStream = new MemoryStream(file.Bytes))
                    using (var zipEntryStream = zipEntry.Open())
                    {
                        originalFileStream.CopyTo(zipEntryStream);
                    }
                }
            }

            return new File()
            {
                Bytes = compressedFileStream.ToArray(),
                FileName = $"Delivery Details.zip"
            };
        }

        private File ToCSVFile(IEnumerable records, string fileName)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(records);
                    }
                }

                bytes = ms.ToArray();
            }

            return new File
            {
                Bytes = bytes,
                FileName = fileName
            };
        }

        private List<DeliveryJobRecord> GetDeliveriesForDate(DateTime deliveryDate)
        {
            return new List<DeliveryJobRecord>()
            {
                new DeliveryJobRecord()
                {
                    StoreName = "Skippad",
                    OrderNo = "55154-4732",
                    Driver = "Veronique Dullard",
                    VehicleNumber = "WDDPK4HA5CF706076",
                    Product = "Naloxone Hydrochloride",
                    Quantity = "21",
                    Customer = "Prisca Wyldbore",
                    Address = "2611 Sutherland Plaza"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Skippad",
                    OrderNo = "76071-1001",
                    Driver = "Jaye Pitfield",
                    VehicleNumber = "JH4KB16628C894418",
                    Product = "Stem Cell Renew",
                    Quantity = "5",
                    Customer = "Lind Staddart",
                    Address = "8429 Portage Street"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Skippad",
                    OrderNo = "52343-053",
                    Driver = "Ivy Dunnet",
                    VehicleNumber = "5UXWX9C58F0253012",
                    Product = "Pioglitazone Hydrochloride",
                    Quantity = "53",
                    Customer = "Jaquelyn Braine",
                    Address = "6 Ridgeway Alley"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Jabbersphere",
                    OrderNo = "36987-1026",
                    Driver = "Verla Kisar",
                    VehicleNumber = "1VWAH7A37DC397707",
                    Product = "Duck Feathers",
                    Quantity = "51",
                    Customer = "Clementius MacIlhagga",
                    Address = "14576 Sauthoff Terrace"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Jabbersphere",
                    OrderNo = "0641-6123",
                    Driver = "Jacquelynn Adamsson",
                    VehicleNumber = "5GAKRAKD4FJ150525",
                    Product = "Ampicillin and Sulbactam",
                    Quantity = "44",
                    Customer = "Tommy Epsley",
                    Address = "5 Graceland Parkway"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Wikizz",
                    OrderNo = "60429-039",
                    Driver = "Marleah Winton",
                    VehicleNumber = "WAUSF98K89A307115",
                    Product = "Ramipril",
                    Quantity = "66",
                    Customer = "Jim Gravells",
                    Address = "4 Steensland Hill"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Wikizz",
                    OrderNo = "66559-2977",
                    Driver = "Kenyon Tynemouth",
                    VehicleNumber = "1FTEW1CM1BK680746",
                    Product = "No7 CC",
                    Quantity =" 94",
                    Customer = "Raphaela Olivet",
                    Address = "6899 Old Gate Avenue"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Dynabox",
                    OrderNo = "41250-200",
                    Driver = "Barrie Bierling",
                    VehicleNumber = "1FTSE3EL6AD939689",
                    Product = "Meijer",
                    Quantity = "98",
                    Customer = "Harman Rive",
                    Address = "03867 Mcbride Road"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Dynabox",
                    OrderNo = "60505-6025",
                    Driver = "Samuele Daintrey",
                    VehicleNumber = "WA1CV94L57D573691",
                    Product = "Cefoxitin",
                    Quantity = "16",
                    Customer = "Merell Noakes",
                    Address = "66545 Dottie Pass"
                },
                new DeliveryJobRecord()
                {
                    StoreName = "Dynabox",
                    OrderNo = "29485-2827",
                    Driver = "Vanya Waind",
                    VehicleNumber = "1N4AA5AP7CC044192",
                    Product = "Vicks DayQuil",
                    Quantity = "40",
                    Customer = "Marj Patis",
                    Address = "74 Declaration Point"
                }
            };
        }
    }
}
