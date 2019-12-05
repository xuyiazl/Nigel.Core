using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Nigel.Core.Helper
{
    public static class ImageMakeThumbnail
    {
        /// <summary>
        /// 生成缩略图 
        /// </summary>
        /// <remarks>Generate(_webFilePath_s, webFilePath_s, 85, 90, imageMakeThumbnailMode.CUT , IsDeleteOriginalImage.YES);//切割缩略图</remarks>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// <param name="iflag">是否删除原图片</param>    
        public static void Generate(string originalImagePath, string thumbnailPath, int width, int height, ImageMakeThumbnailMode mode, IsDeleteOriginalImage isDelete)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ImageMakeThumbnailMode.HW://指定高宽缩放（可能变形）                
                    break;
                case ImageMakeThumbnailMode.W://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ImageMakeThumbnailMode.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ImageMakeThumbnailMode.CUT://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            //g.DrawImage(originalImage, new Rectangle(x, y, w, h), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);

            long[] quality = new long[1];
            quality[0] = 100;

            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();//获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo jpegICI = null;
            for (int i = 0; i < arrayICI.Length; i++)
            {
                if (arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[i];//设置JPEG编码
                    break;
                }
            }
            try
            {
                //以jpg格式保存缩略图
                //bitmap.Save( thumbnailPath , System.Drawing.Imaging.ImageFormat.Jpeg );
                if (jpegICI != null)
                    bitmap.Save(thumbnailPath, jpegICI, encoderParams);
                else
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                if (isDelete == IsDeleteOriginalImage.YES)
                {
                    FileInfo fi = new FileInfo(originalImagePath);
                    if (fi.Exists)
                    {
                        fi.Delete();//删除文件
                        //删除文件成功
                    }
                }
            }
        }
    }

    #region 枚举
    /// <summary>
    /// 图片缩放类型枚举
    /// </summary>
    public enum ImageMakeThumbnailMode
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW,
        /// <summary>
        /// 指定宽，高按比例 
        /// </summary>
        W,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        CUT
    }
    /// <summary>
    /// 是否删除枚举
    /// </summary>
    public enum IsDeleteOriginalImage
    {
        /// <summary>
        /// 删除
        /// </summary>
        YES,
        /// <summary>
        /// 不删除
        /// </summary>
        NO
    }
    /// <summary>
    /// 水印位置枚举
    /// </summary>
    public enum WaterPosition
    {
        /// <summary>
        /// 正上
        /// </summary>
        TOP,
        /// <summary>
        /// 正中间
        /// </summary>
        CENTER,
        /// <summary>
        /// 左边中间
        /// </summary>
        LEFT,
        /// <summary>
        /// 左上角
        /// </summary>
        LEFTUPCORNER,
        /// <summary>
        /// 左下角
        /// </summary>
        LEFTBOMCORNER,
        /// <summary>
        /// 右边中间
        /// </summary>
        RIGHT,
        /// <summary>
        /// 右上角
        /// </summary>
        RIGHTUPCORNER,
        /// <summary>
        /// 右下角
        /// </summary>
        RIGHTBOMCORNER,
        /// <summary>
        /// 底部中间
        /// </summary>
        BOTTOM
    }

    #endregion
}
