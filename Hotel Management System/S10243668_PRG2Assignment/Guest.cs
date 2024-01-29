//========================================================== 
// Student Number : S10243668
// Student Name : Law Jun Jie
// Student Number : S10242004
// Student Name : John Wong
//========================================================== 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Guest
{
    private string name;
    private string passportNum;
    private Stay hotelStay;
    private Membership member;
    private bool isCheckedin;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string PassportNum
    {
        get { return passportNum; }
        set { passportNum = value; }
    }
    public Stay HotelStay
    {
        get { return hotelStay; }
        set { hotelStay = value; }
    }
    public Membership Member
    {
        get { return member; }
        set { member = value; }
    }
    public bool IsCheckedin
    {
        get { return isCheckedin; }
        set { isCheckedin = value; }
    }
    public Guest() { }
    public Guest(string n, string p, Stay s, Membership m)
    {
        Name = n;
        PassportNum = p;
        HotelStay = s;
        Member = m;
    }
    public override string ToString()
    {
        return Name + "\t" + PassportNum + "\t" + HotelStay + "\t" + Member;
    }
}