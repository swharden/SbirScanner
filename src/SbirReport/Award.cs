using static System.Net.WebRequestMethods;

namespace SbirReport;
internal class Award(string[] parts)
{
    public string[] Parts = parts;
    public string Title => Parts[0];
    public string Company => Parts[1];
    public string TrackingNumber => Parts[6];
    public DateOnly Date => string.IsNullOrEmpty(Parts[8])
        ? new DateOnly(int.Parse(Parts[14]), 1, 1)
        : DateOnly.FromDateTime(DateTime.Parse(Parts[8]));
    public string CompanyURL => Parts[22];
    public double Thousand => (int)(double.Parse(Parts[10]) / 1e3);
    public string AwardURL => $"https://www.sbir.gov/awards?keywords={Parts[6]}";
    public string Abstract => Parts[40];
    public string Employees => string.IsNullOrWhiteSpace(Parts[21]) ? "?" : Parts[21];
}

/*
    [0] Award Title
    [1] Company Name
    [2] Agency
    [3] Branch
    [4] Program
    [5] Phase
    [6] Agency Tracking Number
    [7] Contract
    [8] Proposal Award Date
    [9] Contract End Date
    [10] Award Amount
    [11] Award Year
    [12] Topic Code
    [13] Solicitation Number
    [14] Solicitation Year
    [15] Address 1
    [16] Address 2
    [17] City
    [18] State
    [19] ZIP
    [20] Country
    [21] Number Employees
    [22] Company URL
    [23] UEI
    [24] DUNs
    [25] Hubzone Owned
    [26] Women Owned
    [27] Socially Economically Disadvantaged
    [28] PI Name
    [29] PI Title,
    [30] PI Phone
    [31] PI Email
    [32] POC Name
    [33] POC Title
    [34] POC Phone
    [35] POC Email
    [36] RI Name
    [37] RI POC Name
    [38] RI POC Phone
    [39] Research Area Keywords
    [40] Abstract
*/