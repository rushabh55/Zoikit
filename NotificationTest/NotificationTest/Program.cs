using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotificationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int a=Convert.ToInt32(Console.ReadLine());
            UserService.UserServiceClient obj = new UserService.UserServiceClient();
            BuzzService.BuzzServiceClient buzz = new BuzzService.BuzzServiceClient();
            TokenService.TokenServiceClient token = new TokenService.TokenServiceClient();
            token.Open();
            buzz.Open();
            obj.Open();
            while(a!=-1)
            {
                if (a == 1)
                {


                    //USer Follow Test
                   
                    UserService.FollowResult res = obj.FollowUser(52, 53, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res == UserService.FollowResult.Following)
                    {
                        Console.WriteLine("Following");
                    }

                    //Unfollow

                    UserService.FollowResult res1 = obj.UnfollowUser(52, 53, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res1 == UserService.FollowResult.Unfollowed)
                    {
                        Console.WriteLine("UnFollowing");
                    }
                }

                if (a == 2)
                {


                    //USer Follow Test

                    UserService.FollowResult res = obj.FollowUser(53, 52, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res == UserService.FollowResult.Following)
                    {
                        Console.WriteLine("Following");
                    }

                    //Unfollow

                    UserService.FollowResult res1 = obj.FollowUser(52, 50, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res1 == UserService.FollowResult.Following)
                    {
                        Console.WriteLine("Following");
                    }

                    UserService.FollowResult res2 = obj.UnfollowUser(52, 50, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res2 == UserService.FollowResult.Unfollowed)
                    {
                        Console.WriteLine("UnFollowing");
                    }

                    UserService.FollowResult res3 = obj.UnfollowUser(53, 52, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res3 == UserService.FollowResult.Unfollowed)
                    {
                        Console.WriteLine("UnFollowing");
                    }
                }

                if (a == 3)
                {


                    //USer Follow Test

                    UserService.FollowResult res = obj.FollowUser(53, 52, "F68D7DF0-9CB0-4938-9BC6-F932839624F2");
                    if (res == UserService.FollowResult.Following)
                    {
                        Console.WriteLine("Following");
                    }


                    //add buzz
                         
                    buzz.InsertBuzz(52,Guid.NewGuid().ToString(),new BuzzService.Location1{Latitude=17.4156,Longitude=78.4746},null,null,5,null,"5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    //Unfollow

                  

                   
                }

                if (a == 3)
                {


                    //USer Follow Test

                    UserService.FollowResult res = obj.FollowUser(53, 52, "F68D7DF0-9CB0-4938-9BC6-F932839624F2");
                    if (res == UserService.FollowResult.Following)
                    {
                        Console.WriteLine("Following");
                    }


                    //add buzz

                    buzz.InsertBuzz(52, Guid.NewGuid().ToString(), new BuzzService.Location1 { Latitude = 17.4156, Longitude = 78.4746 }, DateTime.Now, null, 5, null, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    //Unfollow




                }

                if (a == 4)
                {
                    UserService.FollowResult res3 = obj.UnfollowUser(53, 52, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res3 == UserService.FollowResult.Unfollowed)
                    {
                        Console.WriteLine("UnFollowing");
                    }
                }

                if (a == 5)
                {
                    UserService.FollowResult res3 = obj.UnfollowUser(53, 52, "5AEF4B9A-6D7E-49FD-A91B-074BE2193A10");
                    if (res3 == UserService.FollowResult.Unfollowed)
                    {
                        Console.WriteLine("UnFollowing");
                    }

                    buzz.UnfollowBuzz(52, 4, "F68D7DF0-9CB0-4938-9BC6-F932839624F2");
                    buzz.FollowBuzz(53, 4, "F68D7DF0-9CB0-4938-9BC6-F932839624F2");

                    buzz.FollowBuzz(52, 4, "F68D7DF0-9CB0-4938-9BC6-F932839624F2");
                }


                a=Convert.ToInt32(Console.ReadLine());
            }

        }
    }
}
