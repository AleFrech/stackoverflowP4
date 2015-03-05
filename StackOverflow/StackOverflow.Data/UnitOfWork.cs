using StackOverflow.Domain.Entities;

namespace StackOverflow.Data
{
    public  class UnitOfWork:IUnitOfWork
    {
        private StackOverflowContext context = new StackOverflowContext();
        private Repository<Account> _AccountRepository;
        private Repository<Question> _QuestionRepository;
        private Repository<Answer> _AnswerRepository;

        public Repository<Account> AccountRepository
        {
            get
            {

                if (this._AccountRepository== null)
                {
                    this._AccountRepository = new Repository<Account>(context);
                }
                return _AccountRepository;
            }
        }

        public Repository<Question> QuestionRepository
        {
            get
            {

                if (this._QuestionRepository == null)
                {
                    this._QuestionRepository = new Repository<Question>(context);
                }
                return _QuestionRepository;
            }
        }
        public Repository<Answer> AnswerRepository
        {
            get
            {

                if (this._AnswerRepository == null)
                {
                    this._AnswerRepository = new Repository<Answer>(context);
                }
                return _AnswerRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }


    }

    public interface IUnitOfWork
    {
        void Save();
    }
}
