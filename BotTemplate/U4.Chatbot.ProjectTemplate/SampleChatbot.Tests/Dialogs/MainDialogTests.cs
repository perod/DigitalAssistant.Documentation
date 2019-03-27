using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using $ext_safeprojectname$.Dialogs;
using $ext_safeprojectname$.Dialogs.Children;
using $ext_safeprojectname$.Models;
using $ext_safeprojectname$.Services.BusinessLogic;
using System.Threading.Tasks;
using U4.Bot.Builder.Test.Fluent;

namespace $ext_safeprojectname$.Tests.Dialogs
{
    [TestClass]
    public class MainDialogTests
    {
        [TestMethod]
        public void MainDialog_happy_path()
        {
            var fluentTest = new MainDialogFluentTest();
            fluentTest.Create()
                .Message(m =>
                {
                    Assert.AreEqual(2, m.Count);

                    //Ensure that the main dialog forwards its operation to the fake child dialog,
                    //and that the message returned from that child dialog is routed back to the user.
                    Assert.AreEqual("Hello, you have now reached the $ext_safeprojectname$.", m[0].Text);
                    Assert.AreEqual("Congratulations, you picked the correct number.", m[1].Text);
                });
        }

        [TestMethod]
        public void MainDialog_child_dialog_failure()
        {
            var fluentTest = new MainDialogFluentTest(false);
            fluentTest.Create()
                .Message(m =>
                {
                    Assert.AreEqual(2, m.Count);

                    //Ensure that the main dialog forwards its operation to the fake child dialog,
                    //and that the message returned from that child dialog is routed back to the user.
                    Assert.AreEqual("Hello, you have now reached the $ext_safeprojectname$.", m[0].Text);
                    Assert.AreEqual("The child dialog failed", m[1].Text);
                });
        }

        private class MainDialogFluentTest
        {
            public MainDialogFluentTest(bool childDialogSuccess = true)
            {
                _childDialogSuccess = childDialogSuccess;
                BusinessLogicServiceMock = new Mock<IBusinessLogicService>();
            }

            private bool _childDialogSuccess;
            public Mock<IBusinessLogicService> BusinessLogicServiceMock { get; private set; }

            public FluentTests<IMessageActivity, MainDialog> Create()
            {
                var retVal = new FluentTests<IMessageActivity, MainDialog>();

                return retVal
                    .SetContainerMocks(b =>
                    {
                        var fakeChildDialog = new FakeChildDialog(BusinessLogicServiceMock.Object, _childDialogSuccess);

                        b.RegisterInstance(BusinessLogicServiceMock.Object).As<IBusinessLogicService>().SingleInstance();
                        b.RegisterInstance(fakeChildDialog).As<ChildDialog>().SingleInstance();
                    })
                    .Interactive()
                    .Message("Start");
            }
        }

        /// <summary>
        /// Fake child dialog. No need to test the flow of this dialog here,
        /// so just resolve with some text that can be evaluated in the test.
        /// </summary>
        private class FakeChildDialog : ChildDialog
        {
            private readonly bool _isSuccess;

            public FakeChildDialog(IBusinessLogicService businessLogicService, bool isSuccess) : base(businessLogicService, true)
            {
                _isSuccess = isSuccess;
            }

            /// <summary>
            /// No need to test the flow of the child dialog here. Just resolve it,
            /// and ensure that the parent dialog presents this message.
            /// </summary>
            public override async Task StartAsync(IDialogContext context)
            {
                if (!_isSuccess)
                {
                    //Somewhat simulates actual behaviour.
                    //Prompt dialogs will post a message
                    await context.PostAsync("The child dialog failed");
                }

                context.Done(new ChildDialogModel {
                    Success = _isSuccess,
                    Message = _isSuccess ? "Congratulations, you picked the correct number." : null
                });
            }
        }
    }
}
