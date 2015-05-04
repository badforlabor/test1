/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/23/2015 10:49:08 PM
 * author : Labor
 * purpose : 
****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SHGame
{
    using LitJson;
    using System;

    public class Person
    {
        // C# 3.0 auto-implemented properties
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class JsonSample
    {
        public static void Main()
        {
            PersonToJson();
            JsonToPerson();
        }

        public static void PersonToJson()
        {
            Person bill = new Person();

            bill.Name = "William Shakespeare";
            bill.Age = 51;
            bill.Birthday = new DateTime(1564, 4, 26);

            string json_bill = JsonMapper.ToJson(bill);

            Console.WriteLine(json_bill);

            JsonHelper.saveObjectToJsonFile(bill, "./test.txt");
        }

        public static void JsonToPerson()
        {
            string json = @"
            {
                ""Name""     : ""Thomas More"",
                ""Age""      : 57,
                ""Birthday"" : ""02/07/1478 00:00:00""
            }";

            Person thomas2 = JsonMapper.ToObject<Person>(json);

            Person thomas = JsonHelper.loadObjectFromJsonFile<Person>("./test.txt");

            Console.WriteLine("Thomas' age: {0}", thomas.Age);
        }
    }
}
