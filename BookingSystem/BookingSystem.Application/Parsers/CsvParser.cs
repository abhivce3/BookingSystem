using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Globalization;
using System.Text;

namespace BookingSystem.Application.Parsers
{
    public static class CsvParser
    {
        public static async Task<List<T>> ParseCsvAsync<T>(byte[] fileByteArray, ClassMap<T> classMap) where T : class
        {

            var stream = new MemoryStream(fileByteArray, writable: false);

            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(classMap); // Register the class map

            var records = new List<T>();
            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }

            await stream.DisposeAsync();

            return records;
        }
    }
}