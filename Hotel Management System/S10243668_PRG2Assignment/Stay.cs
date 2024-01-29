//========================================================== 
// Student Number : S10243668
// Student Name : Law Jun Jie
// Student Number : S10242004
// Student Name : John Wong
//========================================================== 

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Stay
{
    private DateTime checkindate;
    private DateTime checkoutdate;
    public DateTime Checkindate
    {
        get { return checkindate; }
        set { checkindate = value; }
    }
    public DateTime Checkoutdate
    {
        get { return checkoutdate; }
        set { checkoutdate = value; }
    }
    public List<Room> RoomList { get; set; } = new List<Room>();
    public Stay() { }
    public Stay(DateTime ci, DateTime co)
    {
        Checkindate = ci;
        Checkoutdate = co;
    }
    public void AddRoom(Room r)
    {
        RoomList.Add(r);
    }
    public double CalculateTotal(Room r)
    {
        int days = Checkoutdate.Subtract(Checkindate).Days;
        return days * r.CalculateCharge();
    }
    public override string ToString()
    {
        return Checkindate.ToString("dd/MM/yyyy") + " to " + Checkoutdate.ToString("dd/MM/yyyy");
    }
}