using Nigel.Drawing;
using System.Collections.Generic;

namespace Nigel.Core.Uploads.Params
{
    public class SingleImageUploadParam : SingleFileUploadParam
    {
        /// <summary>
        /// 允许上传的文件扩展名
        /// </summary>
        public string[] Extensions { get; set; } = new string[] { "gif", "jpg", "jpeg", "png", "bmp" };

        /// <summary>
        /// 允许上传的图片文件大小，默认1M
        /// </summary>
        public long Size { get; set; } = 1024 * 1024;

        /// <summary>
        /// 裁剪方式（原图不动，若设置了裁剪尺寸，则启用该选项）
        /// </summary>
        public ThumbnailMode CutMode { get; set; }

        /// <summary>
        /// 裁剪缩略图尺寸 item = 300x400
        /// </summary>
        public List<string> Thumbs { get; set; }
    }
}