using DDDLite.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Core.Actors
{
    public static class ActorPaths
    {
        public static ActorMetaData UserCommand = new ActorMetaData("user-command");
        public static ActorMetaData UserQuery = new ActorMetaData("user-query");
    }
}
