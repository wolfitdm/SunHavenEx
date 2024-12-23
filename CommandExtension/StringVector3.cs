using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommandExtension
{
    public class StringVector2
    {
        public float x, y;
        private static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
        public StringVector2()
        {
            this.x = 0;
            this.y = 0;
        }

        public StringVector2(Vector2 v)
        {
            this.x = v.x; 
            this.y = v.y;
        }

        public string ToStringEx()
        {
            string xstring = x.ToString("G99", culture);
            string ystring = y.ToString("G99", culture);
            return xstring + "," + ystring; 
        }

        public string ToStringExVector2(Vector2 v)
        {
            this.x = (float)v.x;
            this.y =(float)v.y;
            return ToStringEx();
        }

        public Vector2 FromStringExVector2(string v)
        {
            if (v == null || v.Length == 0)
            {
                return new Vector2(0, 0);
            }

            string[] comp = v.Split(',');
            float tempX = 0;
            float tempY = 0;
            if (comp.Length == 2)
            {

                try
                {
                    tempX = float.Parse(comp[0], culture);
                }
                catch (Exception)
                {
                    tempX = 0;
                    tempY = 0;
                    return new Vector2(tempX, tempY);
                }
                try
                {
                    tempY = float.Parse(comp[1], culture);
                }
                catch (Exception)
                {
                    tempX = 0;
                    tempY = 0;
                    return new Vector2(tempX, tempY);
                }
            }
            this.x = tempX;
            this.y = tempY;
            return new Vector2(tempX, tempY);         
        }
    }
}
