using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlvHanbai.Web.Class.Utility
{
    public class ExRandomString
    {

        public static string GetRandomString()
        {
            string strPasswordChar_N = "23456789";
            string strPasswordChar_A = "abcdefghijkmopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string str = "";

            // 初期化
            System.Random rnd = new System.Random(
　           (int) (System.DateTime.Now.Ticks % System.Int32.MaxValue));

            // 乱数の書式はANNNNANNNNAAAAAAAAAA(20桁・A=アルファベット/N=数字)
            str = strPasswordChar_A.Substring(rnd.Next(strPasswordChar_A.Length), 1);       //1
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //2
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //3
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //4
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //5
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_A.Length), 1);      //6
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //7
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //8
            str += strPasswordChar_N.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //9
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //10
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //11
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //12
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //13
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //14
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //15
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //16
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //17
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //18
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //19
            str += strPasswordChar_A.Substring(rnd.Next(strPasswordChar_N.Length), 1);      //20
            return str;
        }

    }
}