// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

﻿using System;
using System.Drawing;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace WebsitePanel.Portal
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ResizeImage : IHttpHandler
    {

        public const string URL = "url";

        public const string WIDTH = "width";

        public const string HEIGHT = "height";

		public const int MaxDownloadAttempts = 5;

		public const int BitmapCacheDurationInSeconds = 900;


        private static bool Abort()
        {
            return false;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "image/png";

            string imageUrl = context.Request.QueryString[URL];
            if (!string.IsNullOrEmpty(imageUrl))
            {
				try
				{
					// Create decoded version of the image url
					imageUrl = context.Server.UrlDecode(imageUrl);
					//
					Image img = null;

					try
					{
						WebRequest request = WebRequest.Create(imageUrl);
						WebResponse response = request.GetResponse();
						// Load image stream from the response
						img = new Bitmap(response.GetResponseStream());
					}
					catch (Exception ex)
					{
						Trace.TraceError(ex.StackTrace);
					}

                    int width = Utils.ParseInt(context.Request.QueryString[WIDTH], 20);
                    int height = Utils.ParseInt(context.Request.QueryString[HEIGHT], 20);

                    // calculate new size
                    int h = (img != null) ? img.Height : height;
                    int w = (img != null) ? img.Width : width;
                    int b = Math.Max(h, w);
                    double per = b > Math.Max(width, height) ? (Math.Max(width, height) * 1.0) / b : 1.0;

                    h = (int)(h * per);
                    w = (int)(w * per);

                    Bitmap bitmap = new Bitmap(width, height);
                    Graphics new_g = Graphics.FromImage(bitmap);
                    new_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    new_g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    if (img != null)
                    {
                        // draw image
                        new_g.DrawImage(img, 0, 0, w, h);
                        img.Dispose();
                    }

                    // emit it to the response stream
                    bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);

                    // clean-up
                    bitmap.Dispose();
                    new_g.Dispose();

					// set cache info if image was loaded
                    if (img != null)
                    {
                        context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(BitmapCacheDurationInSeconds));
                        context.Response.Cache.SetCacheability(HttpCacheability.Public);
                        context.Response.Cache.SetValidUntilExpires(true);
                    }

                    // end response
					context.Response.End();
				}
				catch (Exception ex)
				{
					Trace.TraceError(ex.StackTrace);
				}                                                 
            }
        }

		private string GetImageETag(string urlString)
		{
			Encoder stringEncoder;
			int byteCount;
			Byte[] stringBytes;
	 
			// Get string bytes 
			stringEncoder = Encoding.UTF8.GetEncoder();
			byteCount = stringEncoder.GetByteCount(urlString.ToCharArray(), 0, urlString.Length, true);
			stringBytes = new Byte[byteCount];

			stringEncoder.GetBytes(urlString.ToCharArray(), 0, urlString.Length, stringBytes, 0, true);
	 
			//{ Hash string using MD5 and return the hex-encoded hash }
			MD5 md5 = MD5CryptoServiceProvider.Create();
			return @"\" + BitConverter.ToString(md5.ComputeHash(stringBytes)).Replace("-", string.Empty) + @"\";
		}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
