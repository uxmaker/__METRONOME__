using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.WebApp.DAL
{
    public enum Status
    {
        Success,
        Error
    }

    public class RequestResult<TContent> : RequestResult
    {
        public RequestResult( Status status, TContent content, string errorMessage ) : base( status, errorMessage )
        {
        }
        public TContent Content { get; }
    }

    public class RequestResult
    {

        public Status Status { get; }

        public string ErrorMessage { get; }

        public RequestResult( Status status ) : this ( status, string.Empty )
        {

        }

    }
}
