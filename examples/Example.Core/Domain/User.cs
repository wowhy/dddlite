namespace Example.Core.Domain
{
    using DDDLite;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using DDDLite.Commands;
    using DDDLite.Validation;
    using System.Security.Cryptography;
    using System.Text;
    using Commands;

    public class User : AggregateRoot
    {
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        public string Code { get; set; }

        [Required]
        [MaxLength(16)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Password { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(8)]
        public string Salt { get; set; }

        public Guid? EnabledById { get; set; }

        public User EnabledBy { get; set; }

        public override void Create(IAggregateRootCommand command)
        {
            var input = command.AggregateRoot as User;
            var salt = GenerateSalt();
            var password = input.Password;

            input.Salt = salt;
            input.Password = EncryptPassword(password, salt);

            base.Create(command);

            this.Salt = input.Salt;
            this.Password = input.Password;
        }

        public override void Update(IAggregateRootCommand command)
        {
            var input = command.AggregateRoot as User;

            // 此处几项是禁止修改的内容
            input.Id = this.Id;
            input.Code = this.Code;
            input.Password = this.Password;
            input.Salt = this.Salt;

            base.Update(command);
        }

        public void Handle(ChangePasswordCommand command)
        {
            if (command.OldPassword == command.NewPassword)
            {
                throw new CoreValidateException("原始密码与新密码不能相同。");
            }

            var password = EncryptPassword(command.OldPassword, this.Salt);
            if (password != this.Password)
            {
                throw new CoreValidateException("原始密码不正确。");
            }

            this.Salt = GenerateSalt();
            this.Password = EncryptPassword(command.NewPassword, this.Salt);
        }

        public static string GenerateSalt()
        {
            var key = "123456789qwertyuiopasdfghjklzxcvbnm";
            var salt = "";
            var random = new Random((int)DateTime.Now.Ticks);
            for (var i = 0; i < 4; i++)
            {
                salt += key[random.Next(0, key.Length)];
            }

            return salt;
        }

        public static string EncryptPassword(string password, string salt)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"{password}++{salt}")).Select(k => k.ToString("x")));
        }

        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new CoreValidateException("密码不能为空");
            }

            if (password.Length < 6 || password.Length > 16)
            {
                throw new CoreValidateException("密码长度在6-16位之间");
            }
        }
    }
}
