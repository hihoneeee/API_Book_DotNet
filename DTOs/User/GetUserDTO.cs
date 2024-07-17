﻿namespace TestWebAPI.DTOs.User
{
    public class GetUserDTO
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string roleCode { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public string avatar { get; set; }
        public List<GetUserMediaDTO> dataMedia { get; set; }
    }
}
