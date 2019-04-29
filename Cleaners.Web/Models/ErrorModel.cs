using System;

namespace Cleaners.Web.Models
{
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public ErrorModel(int statusCode, string title, string text)
        {
            StatusCode = statusCode;
            Title = title;
            Text = text;
        }

        public ErrorModel()
        {
        }
    }
}