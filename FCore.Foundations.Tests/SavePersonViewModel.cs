namespace FCore.Foundations.Tests
{
    public class SavePersonViewModel<TPerson> : IGetViewModel<TPerson>, ISaveViewModel<TPerson>
        where TPerson : class, IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public SavePersonViewModel()
        {
        }

        public SavePersonViewModel(TPerson person)
        {
            FillViewModel(person);
        }
        public void FillViewModel(TPerson person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Age = person.Age;
        }

        public void UpdateEntity(TPerson person)
        {
            person.FirstName = FirstName;
            person.LastName = LastName;
            person.Age = Age;
        }
    }
}
