using Castle.Windsor;

namespace lifebook.core.eventstore.testing.framework
{
    public interface IInstaller
    {
        void Install(WindsorContainer container);
    }
}