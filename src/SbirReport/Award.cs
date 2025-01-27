namespace SbirReport;
public class Award(string[] parts)
{
    public string[] Parts = parts;
    public string Title => Parts[0];
    public string Company => Parts[1];
    public string TrackingNumber => Parts[6];
    public DateOnly? Date
    {
        get
        {
            string dateString = string.Empty;

            int[] fieldsToTry = [8, 14, 16];

            foreach (int fieldIndex in fieldsToTry)
            {
                if (!string.IsNullOrEmpty(Parts[fieldIndex]))
                {
                    dateString = Parts[fieldIndex];
                    break;
                }
            }

            if (string.IsNullOrEmpty(dateString))
            {
                throw new InvalidDataException("no field contains date");
            }

            if (DateTime.TryParse(dateString, out DateTime dt))
            {
                return DateOnly.FromDateTime(dt);
            }

            if (int.TryParse(dateString, out int year))
            {
                return new DateOnly(year, 1, 1);
            }

            if (DateOnly.TryParse(dateString, out DateOnly date))
            {
                return date;
            }

            return null;
        }
    }
    public string CompanyURL => Parts[23];
    public int? USD => double.TryParse(Parts[17], out double usd) ? (int)usd : null;
    public string AwardURL => $"https://www.sbir.gov/awards?keywords={Parts[6]}";
    public string Abstract => Parts[29];
    public string Employees => string.IsNullOrWhiteSpace(Parts[22]) ? "?" : Parts[22];
}