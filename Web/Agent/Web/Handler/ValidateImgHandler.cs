using LotterySystem.Model;

namespace Agent.Web.Handler
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class ValidateImgHandler : IHttpHandler, IRequiresSessionState
    {
        private int intLength = 4;

        private void CreateCheckCodeImage(HttpContext hc)
        {
            int num;
            StringBuilder builder = new StringBuilder();
            string str = this.RndNum(hc);
            Random random = new Random();
            Bitmap image = new Bitmap((int) Math.Ceiling((double) (str.Length * 25.0)), 0x1d);
            Graphics graphics = Graphics.FromImage(image);
            graphics.InterpolationMode = InterpolationMode.Low;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            Brush brush = new LinearGradientBrush(rect, Color.FromArgb(random.Next(150, 0x100), 0xff, 0xff), Color.FromArgb(0xff, random.Next(150, 0x100), 0xff), (float) random.Next(90));
            graphics.FillRectangle(brush, rect);
            for (num = 0; num < 3; num++)
            {
                Point point = new Point(0, random.Next(image.Height));
                Point point2 = new Point(random.Next(image.Width), random.Next(image.Height));
                Point point3 = new Point(random.Next(image.Width), random.Next(image.Height));
                Point point4 = new Point(image.Width, random.Next(image.Height));
                Point[] points = new Point[] { point, point2, point3, point4 };
                Pen pen = new Pen(Color.Gray, 1f);
                graphics.DrawBeziers(pen, points);
            }
            for (num = 0; num < str.Length; num++)
            {
                string text = str.Substring(num, 1);
                int num2 = random.Next(-15, 15);
                float x = ((image.Width / str.Length) / 2) + ((image.Width / str.Length) * num);
                float y = image.Height / 2;
                Font font = new Font("Consolas", (float) random.Next(0x16, 0x19), FontStyle.Bold);
                SizeF ef = graphics.MeasureString(text, font);
                Matrix matrix = new Matrix();
                matrix.RotateAt((float) num2, new PointF(x, y), MatrixOrder.Append);
                matrix.Shear(random.Next(-10, 10) * 0.03f, 0f);
                graphics.Transform = matrix;
                Brush brush2 = new LinearGradientBrush(rect, Color.FromArgb(random.Next(0, 0x100), 0, 0), Color.FromArgb(0, 0, random.Next(0, 0x100)), (float) random.Next(90));
                graphics.DrawString(str.Substring(num, 1), font, brush2, new PointF(x - (ef.Width / 2f), y - (ef.Height / 2f)));
                graphics.Transform = new Matrix();
            }
            graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            image.Save(hc.Response.OutputStream, ImageFormat.Gif);
            graphics.Save();
            hc.Session["lottery_session_img_code"] = str.ToString();
            graphics.Dispose();
            image.Dispose();
        }

        public void ProcessRequest(HttpContext hc)
        {
//            if ((int.Parse(Utils.Number(3)) % 2) == 0)
//            {
//                new DrawValidationCode().CreateImage(hc.Response.OutputStream, "lottery_session_img_code");
//            }
//            else
//            {
//                Validate validate = new Validate();
//                validate.set_ValidateCodeCount(4);
//                validate.ValidateCodeSize = 13f;
//                validate.ImageHeight = 0x17;
//                validate.DrawColor = Color.BlueViolet;
//                validate.ValidateCodeFont = "Arial";
//                validate.FontTextRenderingHint = false;
//                validate.AllChar = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
//                validate.OutPutValidate("lottery_session_img_code");
//            }
            CreateCheckCodeImage(hc);
        }

        private string RndNum(HttpContext hc)
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            hc.Session["lottery_session_img_code"] = str;
            return str;
        }

        public bool IsReusable =>
            false;
    }
}

