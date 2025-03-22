using BookingSystem.Application.Features.Member.Commands;
using FluentValidation;

namespace BookingSystem.Application.Features.Member.Validators
{
    public class ImportMembersValidator : AbstractValidator<ImportMembersCommand>
    {
        public ImportMembersValidator()
        {
            RuleFor(x => x.FileByteArray)
            .NotNull().WithMessage("File data cannot be null.")
            .NotEmpty().WithMessage("File data cannot be empty.")
            .Must(BeValidCsvFile).WithMessage("Invalid file format. Only CSV with at least one data row is allowed.");
        }

        // ✅ Method to validate if the byte array is a valid CSV file
        private bool BeValidCsvFile(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
                return false;

            try
            {
                using var stream = new MemoryStream(fileBytes);
                using var reader = new StreamReader(stream);

                // Read first row to check if it's a valid CSV
                string? headerLine = reader.ReadLine();
                return !string.IsNullOrWhiteSpace(headerLine) && headerLine.Contains(",");
            }
            catch
            {
                return false; // If any error occurs, assume it's not a valid CSV
            }
        }
    }
}