//========================================================== 
// Student Number : S10243668
// Student Name : Law Jun Jie
// Student Number : S10242004
// Student Name : John Wong
//========================================================== 


using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;

void displaymenu ()
{
	Console.WriteLine("--------------- MENU -----------------\n" +
	"[1] List all guests\n" +
	"[2] List all available rooms\n" +
	"[3] Register guest\n" +
	"[4] Check-in guest\n" +
    "[5] Check-out guest\n" +
    "[6] Display stay details of a guest\n" +
	"[7] Extends the stay by numbers of day\n" +
    "[8] Display monthly charged amounts for the year\n" +
    "[9] Display Reviews\n" +
    "[0] Exit\n" +
	"--------------------------------------");
}

List<Guest> guestlist = new List<Guest>();
List<Stay> staylist = new List<Stay>();
List<Membership> membershiplist = new List<Membership>();
List<Room> roomlist = new List<Room>();
List<Review> reviewlist = new List<Review>();
List<Int32> UnavailableRooms = new List<Int32>();
List<string> IC = new List<string>();
Dictionary<int, double> monthlyrevenue = new Dictionary<int, double>();
List<string> month = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
string guestHeading = "";
string stayHeading = "";
string membershipHeading = "";
string standardRoomHeading = "";
string deluxeRoomHeading = "";
string reviewHeading = "";
CreateRoomList();
CreateMembershipList();
CreateGuestList();
CreateStayList();
CreateReviewList();
FindUnavaillableRooms();
Initializemonthlyrevenue();

void CreateRoomList()
{
	using (StreamReader sr = new StreamReader("Rooms.csv"))
	{
		string? s = sr.ReadLine();
		if (s != null)
		{
			string[] heading = s.Split(',');
			standardRoomHeading = $"{heading[1]}\t{heading[2]}\t{heading[3]}\tAvailable\tWifi\tBreakfast";
			deluxeRoomHeading = $"{heading[1]}\t{heading[2]}\t{heading[3]}\tAvailable\tExtraBed";
		}
		while ((s = sr.ReadLine()) != null)
		{
			string[] data = s.Split(',');
			if (data[0] == "Standard")
			{
				Room r = new Standard_Room(Convert.ToInt32(data[1]), data[2], Convert.ToInt32(data[3]), true);
				roomlist.Add(r);
			}
			else
			{
				Room r = new Deluxe_Room(Convert.ToInt32(data[1]), data[2], Convert.ToInt32(data[3]), true);
				roomlist.Add(r);
			}
		}
	}
}
Stay CreateStay(string passportnum)
{
    string[] csvLines = File.ReadAllLines("Stays.csv");
    for (int j = csvLines.Length - 1; j > 0; j--)
    {
        bool newic = true;
        string[] data = csvLines[j].Split(',');
        foreach (string ic in IC)
        {
            if (data[0] == ic)
                newic = false;
        }
        if (data[1] == passportnum && newic == true)
        {
            IC.Add(data[1]);
            Stay stay = new Stay(Convert.ToDateTime(data[3]), Convert.ToDateTime(data[4]));
            for (int i = 0; i < data.Length; i++)
            {
                if (int.TryParse(data[i], out int id))
                {
                    foreach (Room r in roomlist)
                    {
                        if (r.RoomNumber == id)
                        {
                            if (r is Standard_Room)
                            {
                                Standard_Room s_r = new Standard_Room(r.RoomNumber, r.BedConfigurration, r.DailyRate, r.IsAvail);
                                s_r.RequireWifi = Convert.ToBoolean(data[i + 1]);
                                s_r.RequireBreakfast = Convert.ToBoolean(data[i + 2]);
                                stay.AddRoom(s_r);
                            }
                            else
                            {
                                Deluxe_Room d_r = new Deluxe_Room(r.RoomNumber, r.BedConfigurration, r.DailyRate, r.IsAvail);
                                d_r.AdditionalBed = Convert.ToBoolean(data[i + 3]);
                                stay.AddRoom(d_r);
                            }
                        }
                    }
                }
            }
            return stay;
        }
    }
    Stay newstay = new();
    return newstay;
}


void CreateStayList()
{
    string[] csvLines = File.ReadAllLines("Stays.csv");
    string[] heading = csvLines[0].Split(',');
    stayHeading = $"{heading[3]}   {heading[4]}";
    for (int j = csvLines.Length - 1; j > 0; j--)
    {
        bool newstay = true;
        string[] data = csvLines[j].Split(',');
        foreach (Guest g in guestlist)
		{
			if (g.PassportNum == data[1] && g.HotelStay.Checkindate == Convert.ToDateTime(data[3]) && g.HotelStay.Checkoutdate != Convert.ToDateTime(data[4]))
				newstay = false;
		}
		if (newstay == false)
			continue;
		else
		{
            Stay stay = new Stay(Convert.ToDateTime(data[3]), Convert.ToDateTime(data[4]));
            for (int i = 0; i < data.Length; i++)
            {
                if (int.TryParse(data[i], out int id))
                {
                    foreach (Room r in roomlist)
                    {
                        if (r.RoomNumber == id)
                        {
                            if (r is Standard_Room)
                            {
                                Standard_Room s_r = new Standard_Room(r.RoomNumber, r.BedConfigurration, r.DailyRate, r.IsAvail);
                                s_r.RequireWifi = Convert.ToBoolean(data[i + 1]);
                                s_r.RequireBreakfast = Convert.ToBoolean(data[i + 2]);
                                stay.AddRoom(s_r);
                            }
                            else
                            {
                                Deluxe_Room d_r = new Deluxe_Room(r.RoomNumber, r.BedConfigurration, r.DailyRate, r.IsAvail);
                                d_r.AdditionalBed = Convert.ToBoolean(data[i + 3]);
                                stay.AddRoom(d_r);
                            }
                        }
                    }
                }
            }
            staylist.Add(stay);
        }
    }
}

void CreateMembershipList()
{
		using (StreamReader sr = new StreamReader("Guests.csv"))
	{
		string? s = sr.ReadLine();
		if (s != null)
		{
			string[] heading = s.Split(',');
			membershipHeading = $"{heading[2]}\t{heading[3]}";
		}
		while ((s = sr.ReadLine()) != null)
		{
			string[] data = s.Split(',');
			Membership m = new Membership(data[2], Convert.ToInt32(data[3]));
			membershiplist.Add(m);
		}
	}
}

void CreateGuestList()
{
	using (StreamReader sr = new StreamReader("Guests.csv"))
	{
		string? s = sr.ReadLine();
		int i = 0;
		if (s != null)
		{
			string[] heading = s.Split(',');
			guestHeading = $"{heading[0]}\t{heading[1]}\tStay\t\t\t\tMember\t\tIsCheckedin";
		}
		while ((s = sr.ReadLine()) != null)
		{            
			string[] data = s.Split(',');
			Stay stay = CreateStay(data[1]);
            Guest g = new Guest(data[0], data[1], stay, membershiplist[i]);
            i++;
            g.IsCheckedin = validate_checkin(stay.Checkindate, stay.Checkoutdate);
            guestlist.Add(g);
        }
	}
}

void CreateReviewList()
{
    using (StreamReader sr = new StreamReader("Review.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null)
        {
            string[] heading = s.Split(',');
            reviewHeading = $"{heading[2]}\t{heading[3]}\t{heading[4]}\t{heading[5]}";
        }
        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            Review r = new Review(Convert.ToInt32(data[2]), Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), data[5]);
            reviewlist.Add(r);
        }
    }
}


void AddStaydetails(Guest g)
{
    string staydata = $"{g.Name},{g.PassportNum},{g.IsCheckedin.ToString().ToUpper()},{g.HotelStay.Checkindate.ToString("dd/MM/yyyy")},{g.HotelStay.Checkoutdate.ToString("dd/MM/yyyy")}";
    foreach (Room r in g.HotelStay.RoomList)
    {
        if (r is Standard_Room)
        {
            Standard_Room s_r = (Standard_Room)r;
            staydata = staydata + $",{r.RoomNumber},{s_r.RequireWifi.ToString().ToUpper()},{s_r.RequireBreakfast.ToString().ToUpper()}, FALSE";
		}
        else
        {
            Deluxe_Room d_r = (Deluxe_Room)r;
            staydata = staydata + $",{r.RoomNumber},FALSE,FALSE, {d_r.AdditionalBed.ToString().ToUpper()}";
        }
    }
    using (StreamWriter sw = new StreamWriter("Stays.csv", true))
    {
        sw.WriteLine(staydata);
    }
}

void FindUnavaillableRooms()
{
    foreach (Guest g in guestlist)
    {
		if (g.IsCheckedin == true)
		{
			foreach (Room r in g.HotelStay.RoomList)
			{
                UnavailableRooms.Add(r.RoomNumber);
            }
		}
    }
}

void Initializemonthlyrevenue()
{
    for (int i = 0; i < month.Count; i++)
    {
        monthlyrevenue.Add(i+1, 0);
    }
}

void AddReview(Guest g, bool option)
{
    if (option == true)
    {
        int Rr = ratingcheck("Room");
        int Fr = ratingcheck("Facilities");
        int Sr = ratingcheck("Service");
        Console.Write("Leave anymore feedback: ");
        string f = Console.ReadLine();
        Review r = new Review(Rr, Fr, Sr, f);
		Console.WriteLine("Our outmost gratitude for leaving a review on your stay");
        reviewlist.Add(r);
        using (StreamWriter sw = new StreamWriter("Review.csv", true))
        {
            string reviews = $"{g.Name},{g.PassportNum},{r.Roomrating},{r.FacilitiesRating},{r.ServiceRating},{r.Feedback}";
            sw.WriteLine(reviews);
        }
    }
    else
        Console.WriteLine("Have a nice day!");
}



//1
void DisplayGuest()
{
	Console.WriteLine(guestHeading);
	foreach (Guest g in guestlist)
	{
		validate_checkin(g.HotelStay.Checkindate, g.HotelStay.Checkoutdate);
        Console.WriteLine(g + $"\t{g.IsCheckedin}");
	}
}

//2
void DisplayRooms()
{
	using (StreamReader sr = new StreamReader("Rooms.csv"))
	{
		string? s = sr.ReadLine(); 
		if (s != null)
		{
			string[] heading = s.Split(',');
			Console.WriteLine("{0,-10}  {1,-10}  {2,-10}  {3,-10}",
				heading[0], heading[1], heading[2], heading[3]);
		}

		while ((s = sr.ReadLine()) != null)
		{
			bool x = false;
			string[] rooms = s.Split(',');
			for (int i = 0; i < UnavailableRooms.Count; i++)
			{
				if (UnavailableRooms[i] == Convert.ToInt32(rooms[1]))
				{
					x = true;
					foreach (Room room in roomlist)
					{
						if (UnavailableRooms[i] == room.RoomNumber)
						{
							room.IsAvail = false;
						}
					}
					break;
				}
			}
			if (x == false)
			{
				Console.WriteLine("{0,-10}  {1,-10}  {2,-16}  {3,-10}",
					rooms[0], rooms[1], rooms[2], rooms[3]);
			}
		}
	}
}

//3
void RegisterGuest()
{
	string Name = registername();
    string PassportNum = passportcheck(true);
	Stay s = new();
	Membership m = new Membership("Ordinary", 0);
	string guest = Name + "," + PassportNum + "," + m.Status + "," + m.Points;
	using (StreamWriter sw = new StreamWriter("Guests.csv", true))
	{
		sw.WriteLine(guest);
	}
	Guest g = new Guest(Name, PassportNum.ToUpper(), s, m);
	guestlist.Add(g);
	Console.WriteLine("Guest registered!");
}

//4
void Checkinguests()
{
	while (true)
	{
		DisplayGuest();
		Guest g = checkguest();
		DateTime check_in_date = dateIncheck();
		DateTime check_out_date = dateOutcheck(check_in_date);
		Stay newStay = new Stay(check_in_date, check_out_date);
		while (true)
		{
			DisplayRooms();
			Room r = roomcheck();
			newStay.AddRoom(r);
			UnavailableRooms.Add(r.RoomNumber);
			if (r is Standard_Room)
			{
				Standard_Room sr = (Standard_Room)r;
				Console.WriteLine("Selected a standard room.");
                sr.RequireWifi = YNcheck("wifi");
                sr.RequireBreakfast = YNcheck("breakfast");
			}
			else
			{
				if (r is Deluxe_Room)
				{
					Deluxe_Room dr = (Deluxe_Room)r;
					dr.AdditionalBed = YNcheck("an additional bed");
				}
			}
			bool morerooms = YNcheck("to book more rooms");
			if (morerooms == true)
				continue;
			else
			{
				g.HotelStay = newStay;
				g.IsCheckedin = true;
                Console.WriteLine("You are checked in! have a nice stay.");
				break;
			}            
        }
        staylist.Add(g.HotelStay);
        AddStaydetails(g);
        break;
	}
}

//5 -- Advanced feature
void Checkoutguests()
{
    while (true)
    {
        DisplayGuest();
        Guest g = checkguest();
        if (g.IsCheckedin == true)
        {
            g.HotelStay.Checkoutdate = DateTime.Now;
            int diff = g.HotelStay.Checkoutdate.Subtract(g.HotelStay.Checkindate).Days;
            int total_points = g.Member.Points;
            double total_bills = 0;
            foreach (Room r in g.HotelStay.RoomList)
            {
                total_bills += r.CalculateCharge() * diff;
            }
            Console.WriteLine("\nTotal Bill\n" + total_bills);
            Console.WriteLine("\nStatus\t Points\n" + g.Member + "\n");
            if (g.Member.Points > 0)
            {
                int p = pointscheck(g);
				g.Member.Points -= p;
                total_bills -= p;
                Console.WriteLine("\nTotal Bill\n" + total_bills);
            }
            Console.WriteLine("Press any key to proceed");
            string proceed = Console.ReadLine();
            double pointearned = total_bills / 10;
            total_points += Convert.ToInt32(pointearned);
            g.Member.EarnPoints(total_bills);
            if (total_points > 200)
                g.Member.Status = "Gold";
            else if (total_points > 100)
                g.Member.Status = "Silver";
            g.IsCheckedin = false;
            Console.WriteLine("Payment successful! Guest has checked out.");
			AddStaydetails(g);
            bool YN = YNcheck("to leave a review");
            AddReview(g, YN);
            break;
        }
        else
            Console.WriteLine("Guest not Checked In!");        
    }
}

//6
void DisplayStayDetails()
{
	Guest g = checkguest();
	Console.WriteLine("\n" + stayHeading + "\n" + g.HotelStay);
	int diff = g.HotelStay.Checkoutdate.Subtract(g.HotelStay.Checkindate).Days;
	double total_cost = 0;
	Console.WriteLine("\nNumber of days\n" + diff + "\n");
	foreach (Room r in g.HotelStay.RoomList)
	{
		if (r is Standard_Room)
			Console.WriteLine(standardRoomHeading + "\tCalculateCharge");
		else
			Console.WriteLine(deluxeRoomHeading + "\tCalculateCharge");
		Console.WriteLine(r + $"\t\t{r.CalculateCharge() * diff}");
		total_cost += r.CalculateCharge() * diff;
	}
	Console.WriteLine("\nTotal Cost\n" + total_cost + "\n");
}

//7
void ExtendStay()
{
	while (true)
	{
        Guest g = checkguest();
        if (g.IsCheckedin == true)
        {
			int days = extendstaycheck();
            g.HotelStay.Checkoutdate = g.HotelStay.Checkoutdate.AddDays(days);
			AddStaydetails(g);
            break;
        }
        else
            Console.WriteLine("Guest not Checked In!");
    }
}

//8 -- Advanced feature
void DisplayRevenue()
{
	int year = yearcheck();
    foreach (Stay s in staylist)
    {
        if (s.Checkoutdate.Year == year)
        {
            foreach (Room r in s.RoomList)
            {
				monthlyrevenue[s.Checkoutdate.Month] += s.CalculateTotal(r);
            }
        }
    }
    for (int i = 0; i < month.Count; i++)
    {
        Console.WriteLine("{0,-20}:${1,0}", month[i], monthlyrevenue[i+1]);
    }
    foreach (KeyValuePair<int, double> kvp in monthlyrevenue)
    {
        monthlyrevenue[kvp.Key] = 0;
    }
}

//9
void DisplayReviews()
{
    Console.WriteLine(reviewHeading);
    foreach (Review r in reviewlist)
    {
        Console.WriteLine(r);
    }
}


while (true)
{
    displaymenu();
	int option = optioncheck();
	if (option == 0)
	{
		Console.WriteLine("Bye");
		break;
	}
	else if (option == 1)
		DisplayGuest();
	else if (option == 2)
		DisplayRooms();
	else if (option == 3)
		RegisterGuest();
	else if (option == 4)
		Checkinguests();
    else if (option == 5)
        Checkoutguests();
    else if (option == 6)
	{
		DisplayGuest();
		DisplayStayDetails();
	}
	else if (option == 7)
	{
        DisplayGuest();
        ExtendStay();
    }
    else if (option == 8)
    {
		DisplayRevenue();
    }
    else if (option == 9)
    {
        DisplayReviews();
    }
}

int optioncheck ()
{
	while (true)
	{
		try
		{
			Console.Write("Enter your option : ");
			int option = Convert.ToInt32(Console.ReadLine());
			if (option > 9)
			{
				Console.WriteLine("Invalid option! Please enter a number from 0-9.");
				continue;
			}
			return option;
		}
		catch (System.FormatException)
		{
			Console.WriteLine("Please enter a number");
		}
		catch (System.OverflowException)
		{
			Console.WriteLine("Please enter a number from 0-9");
		}
	}
}

string namecheck()
{
	while (true)
	{
		Console.Write("Enter name: ");
		string Name = Console.ReadLine();
		if (int.TryParse(Name, out int n) is true)
		{
			Console.WriteLine("Please enter a proper name.");
			continue;
		}
		foreach (Guest g in guestlist)
		{
			if (Name.ToUpper() == g.Name.ToUpper())
			{
				return g.Name;
			}            
		}
		Console.WriteLine("Name not found!");
		continue;
	}
}

string passportcheck(bool tf)
{
	while (true)
	{
		Console.Write("Enter passport number: ");
		string passportnum = Console.ReadLine();
		if (passportnum.Count() != 9)
		{
			Console.WriteLine("Invalid passport! Please enter a 9 character passport number");
			continue;
		}
		if (tf == false)
		{
			foreach (Guest g in guestlist)
			{
				if (passportnum.ToUpper() == g.PassportNum.ToUpper())
				{
					return g.PassportNum;
				}
			}
			Console.WriteLine("Passport number not found!");
			continue;
		}
		else
			return passportnum.ToUpper();
	}
}

Guest checkguest ()
{
	while (true)
	{
		string Name = namecheck();
		string PassportNum = passportcheck(false);
		foreach (Guest g in guestlist)
		{
			if (Name == g.Name && PassportNum == g.PassportNum)
			{
				return (g);
			}
		}
		Console.WriteLine("Guest not found!");
	}
}

Room roomcheck()
{
	while (true)
	{
		try
		{
			Console.Write("Please enter room number: ");
			int roomnum = Convert.ToInt32(Console.ReadLine());
			bool room = true;
			if (roomnum.ToString().Length != 3)
			{
				Console.WriteLine("Please enter a 3-digit room number");
				continue;
			}
			foreach (Room r in roomlist)
			{
				if (roomnum == r.RoomNumber)
				{
					foreach (int un in UnavailableRooms)
					{
						if (un == roomnum)
						{
							Console.WriteLine("Room is unavailable. Please enter another room number.");
							room = false;
						}
					}
					if (room == true)
						return r;
				}
			}
			if (room == false)
				continue;
			Console.WriteLine("Please enter a valid room number.");
			continue;
		}
		catch (System.FormatException)
		{
			Console.WriteLine("Please enter a number");
		}
		catch (System.OverflowException)
		{
			Console.WriteLine("Please enter a 3-digit room number");
		}
	}
}

DateTime dateIncheck()
{
	while (true)
	{
		try
		{
			Console.Write("Enter check in date (dd/mm/yyyy): ");
			DateTime check_in_date = Convert.ToDateTime(Console.ReadLine());
			if (check_in_date < DateTime.Now)
			{
				Console.WriteLine("The date you entered has already passed.Please enter a valid date.");
			}
			else
				return check_in_date;
		}
		catch (System.FormatException)
		{
			Console.WriteLine("Date must be formatted dd/mm/yyyy");
		}
	}
}

DateTime dateOutcheck(DateTime check_in_date)
{
	while (true)
	{
		try
		{
			Console.Write("Enter check out date (dd/mm/yyyy): ");
			DateTime check_out_date = Convert.ToDateTime(Console.ReadLine());
			if (check_out_date < check_in_date)
			{
				Console.WriteLine("The check out date cannot be earlier than the check in date.");
			}
			else
				return check_out_date;
		}
		catch (System.FormatException)
		{
			Console.WriteLine("Date must be in the form of dd/mm/yyyy");
		}
	}
}


int pointscheck(Guest g)
{
    while (true)
    {
        try
        {
            Console.Write("How many points would you want to use: ");
            int p = Convert.ToInt32(Console.ReadLine());
            bool redeem = g.Member.RedeemPoints(p);
            if (redeem == false)
            {
                Console.WriteLine("Please enter a number that is less than " + g.Member.Points);
                continue;
            }
            return p;
        }
        catch (System.FormatException)
        {
            Console.WriteLine("Please enter a number");
        }
        catch (System.OverflowException)
        {
            Console.WriteLine("Please enter a number that is less than " + g.Member.Points);
        }
    }
}

int yearcheck()
{
    while (true)
    {
        try
        {
            Console.Write("Enter the year: ");
            int Year = Convert.ToInt32(Console.ReadLine());
            if (Year < 2022)
            {
                Console.WriteLine("The company opened in 2022 and do not have any data before year 2022.");
                continue;
            }
            else if (Year > DateTime.Now.Year)
            {
                Console.WriteLine("The year has yet to come.Please enter a number that is less than " + DateTime.Now.Year);
                continue;
            }
            return Year;
        }
        catch (System.FormatException)
        {
            Console.WriteLine("Please enter a number");
        }
        catch (System.OverflowException)
        {
            Console.WriteLine($"Please enter a number from 2022 to {DateTime.Now.Year}");
        }
    }
}

bool validate_checkin (DateTime cIn, DateTime cOut)
{
	if (cIn <= DateTime.Now && cOut >= DateTime.Now)
		return true;
	else 
		return false;
}

bool YNcheck(string s)
{
	while (true)
	{

		Console.Write($"Would you like {s}?[Y/N] ");
		string wc = Console.ReadLine().ToUpper();
		if (wc == "Y")
			return true;
		else if (wc == "N")
            return false;
		else
		{
			Console.WriteLine("Please enter 'Y' or 'N' only");
			continue;
		}
	}
		
}

int extendstaycheck()
{
    while (true)
    {
        try
        {
            Console.Write("How many days would you like to extend your stay by: ");
            int days = Convert.ToInt32(Console.ReadLine());
            if (days <= 0)
            {
                Console.WriteLine("Please enter a number that is greater than 0");
                continue;
            }
            return days;
        }
        catch (System.FormatException)
        {
            Console.WriteLine("Please enter a number");
        }
        catch (System.OverflowException)
        {
            Console.WriteLine("Please enter an appropriate number");
        }
    }
}

int ratingcheck(string s)
{
    while (true)
    {
        try
        {
            Console.Write($"Rate the {s}(1-5): ");
            int rating = Convert.ToInt32(Console.ReadLine());
            if (rating > 5 || rating < 1)
            {
                Console.WriteLine("Invalid option! Please enter a number from 1-5.");
                continue;
            }
            return rating;
        }
        catch (System.FormatException)
        {
            Console.WriteLine("Please enter a number");
        }
        catch (System.OverflowException)
        {
            Console.WriteLine("Please enter a number from 1-5");
        }
    }
}

string registername()
{
    while (true)
    {
        Console.Write("Enter name: ");
        string Name = Console.ReadLine();
        if (int.TryParse(Name, out int n) is true || Name == null)
        {
            Console.WriteLine("Please enter a proper name.");
            continue;
        }
        return Name;
    }
}