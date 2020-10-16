using System;
using System.Web;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UserDashboard.Models;
namespace UserDashboard.Models
{
    public class User: IdentityUser
    {
        public DateTime CreatedAt {get; set;}
        [DataType(DataType.Password)]
        [Required]
        public string Password {get; set;}
        [Required]
        public string Description {get; set;}
        [Required]
        [MinLength(4)]
        public string FirstName {get; set;}
        [Required]
        [MinLength(4)]
        public string LastName {get; set;}
        public List<Message> messages {get; set;}
        public List<Comment> comments {get; set;}    
        [NotMapped]   
        public IList<string> role {get; set;} 
    }
     public class Message
    {
        [Key]
        public int MessageId {get; set;}
        public User user {get; set;}
        public DateTime CreatedAt {get; set;}
        public string Author {get; set;}
        public List<Comment> comment {get; set;}
        public string content {get; set;}
        [NotMapped]
        public TimeSpan duration {get; set;}
        
        
    }
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public Message message {get; set;}
        public DateTime CreatedAt {get; set;}
        public string Author {get; set;}
        public User user {get; set;}
        public string content {get; set;}
        [NotMapped]
        public TimeSpan duration {get; set;}

}

    public class UserRoles
    {
        [Required]
        public string RoleName {get; set;}
    }
    public class Post
    {
        public User user {get; set;}
        public Message message {get; set;}
        public Comment comment {get; set;}
        public int MessageId {get; set;}
    }
    public class Login
    {
        [Required]
        public string UserName {get; set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password {get; set;}
    }

}