
namespace Ueels
{
    struct Vector4Int
    {
        public int x, y, z, w;
        public static Vector4Int zero=new Vector4Int(0,0,0,0);
        public Vector4Int(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

}
