using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Domain;

namespace WebApplication2.Services
{
    public class LocalFormRepoistory : IFormRepository
    {
        private List<Form> _forms;
        public LocalFormRepoistory()
        {
            _forms = new List<Form>
            {
                new Form()
                {
                    ID = 1,
                    FormName = "Test Form",
                    Sections = new List<Page>
                    {
                        new Page
                        {
                            ID = 1,
                            Name = "Page 1 of 2",
                            Questions = new List<Question>
                            {
                                new Question
                                {
                                    ID = 1,
                                    Key = "firstName",
                                    Value = "What is your first name",
                                    ExpectedResponseType = typeof(string)
                                },
                                new Question
                                {
                                    ID = 2,
                                    Key = "lastName",
                                    Value = "What is your last name",
                                    ExpectedResponseType = typeof(string)
                                }
                            }
                        },
                        new Page
                        {
                            ID = 2,
                            Name = "Page 2 of 2",
                            Questions = new List<Question>
                            {
                                new Question
                                {
                                    ID = 3,
                                    Key = "primaryAddress",
                                    Value = "What is your full address",
                                    ExpectedResponseType = typeof(string)
                                },
                                new Question
                                {
                                    ID = 4,
                                    Key = "dateOfBirth",
                                    Value = "What is your date of birth",
                                    ExpectedResponseType = typeof(string)
                                }
                            }
                        }
                    }
                }
            };
        }
        public void Add(Form form)
        {
            _forms.Add(form);
        }

        public List<Form> GetAll() => _forms;
    }

    public class Form
    {
        public int ID { get; set; }
        public Application application { get; set; }
        [Display(Name = "Form Name")]
        public string FormName { get; set; }
        public List<Page> Sections { get; set; }
    }

    public class Application
    {
        public int ID { get; set; }
        public string AppType { get; set; }
    }

    public class Page
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }

    }

    public class Question
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Type ExpectedResponseType { get; set; }
    }
}
