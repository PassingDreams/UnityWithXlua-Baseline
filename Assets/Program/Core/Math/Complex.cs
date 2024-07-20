using UnityEngine;

namespace Ants.Utilities
{
    public class Complex
    {
        public float re, im;
        public float sqrMagnitude=>re * re + im * im;
        public float magnitude => Mathf.Sqrt(sqrMagnitude);
        public Complex normalized=>this/magnitude;
        public Complex conjugate=>new Complex(re,-im);
        public Vector2 toVec2=>new Vector2(re,im);
        public Complex()
        {
            re = 0;
            im = 0;
        }

        public Complex(float re, float im)
        {
            this.re = re;
            this.im = im;
        }
        public Complex(Complex c)
        {
            re=c.re;
            im = c.im;
        }

        public Complex(Vector2 v)
        {
            re = v.x;
            im = v.y;
        }



        public static Complex operator+(Complex c1,Complex c2)
        {
            return new Complex(c1.re+c2.re,c1.im+c2.im);
        }
        public static Complex operator-(Complex c)
        {
            return new Complex(-c.re,-c.im);
        }
        public static Complex operator-(Complex c1,Complex c2)
        {
            return new Complex(c1.re-c2.re,c1.im-c2.im);
        }
        public static Complex operator*(Complex c,float x)
        {
            return new Complex(c.re*x,c.im*x);
        }
        public static Complex operator*(float x,Complex c)
        {
            return c*x;
        }
        public static Complex operator*(Complex c1,Complex c2)
        {
            return new Complex(c1.re*c2.re-c1.im*c2.im,c1.re*c2.im+c1.im*c2.re);
        }
        public static Complex operator/(Complex c,float x)
        {
            return c*(1/x);
        }
        public static Complex operator/(Complex c1,Complex c2)
        {
            return c1*c2.conjugate/c2.magnitude;
        }

        /**
         *rad need to be revert into angle
         */
        public void setOnPolarSystem(float magnitude,float angle)
        {
            re = magnitude * Mathf.Cos(angle);
            im = magnitude * Mathf.Sin(angle);
        }
        
    }

}
