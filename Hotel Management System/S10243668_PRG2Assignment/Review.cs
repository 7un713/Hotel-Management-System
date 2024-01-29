using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

internal class Review
{
    private int roomRating;
    private int facilitiesRating;
    private int serviceRating;
    private string feedback;
    public int Roomrating
    {
        get { return roomRating; }
        set { roomRating = value; }
    }
    public int FacilitiesRating
    {
        get { return facilitiesRating; }
        set { facilitiesRating = value; }
    }
    public int ServiceRating
    {
        get { return serviceRating; }
        set { serviceRating = value; }
    }
    public string Feedback
    {
        get { return feedback; }
        set { feedback = value; }
    }
    public Review() { }
    public Review(int r, int f, int s, string F)
    {
        Roomrating = r;
        FacilitiesRating = f;
        ServiceRating = s;
        Feedback = F;
    }
    public override string ToString()
    {
        return $"{Roomrating}\t\t{FacilitiesRating}\t\t\t{ServiceRating}\t\t{Feedback}";
    }
}
