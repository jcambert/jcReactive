using System;

namespace jcReactive.Common
{
    internal class ReactiveDbException : Exception
    {
        public static ReactiveDbException NoDataContractSpecified = new ReactiveDbException("ReactiveObject Must have DataContractAttribute to work correctly");
        public static ReactiveDbException NoKeySpecified = new ReactiveDbException("ReactiveObject Must have one KeyAttribute exactly");

        public ReactiveDbException(string message) : base(message)
        {

        }

    }

}