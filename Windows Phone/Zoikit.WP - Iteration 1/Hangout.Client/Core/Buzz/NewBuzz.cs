using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hangout.Client.Core.Buzz
{
    class NewBuzz
    {
        public Guid BuzzID { get; set; }

        public Guid CityID { get; set; }

        public delegate void NewBuzzResultHelper(bool newbuzz);

        public event NewBuzzResultHelper NewBuzzResult;


        public void Init()
        {
            Services.BuzzServiceClient.HasNewBuzzCompleted += BuzzServiceClient_HasNewBuzzCompleted;
        }

        void BuzzServiceClient_HasNewBuzzCompleted(object sender, BuzzService.HasNewBuzzCompletedEventArgs e)
        {
            if(e.Error==null)
            {
                 if(NewBuzzResult!=null)
                 {
                     NewBuzzResult(e.Result);
                 }
            }
        }
        public void LoopForNewBuzz()
        {
            while (true)
            {
                Services.BuzzServiceClient.HasNewBuzzAsync(Core.User.User.UserID, CityID, BuzzID, Core.User.User.ZAT);
                Thread.Sleep(new TimeSpan(0, 0, 30));
            }
        }
    }
}
