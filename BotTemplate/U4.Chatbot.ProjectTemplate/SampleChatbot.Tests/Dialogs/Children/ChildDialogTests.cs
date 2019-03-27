using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using $ext_safeprojectname$.Dialogs.Children;
using $ext_safeprojectname$.Models;
using $ext_safeprojectname$.Services.BusinessLogic;
using U4.Bot.Builder.Test.Extensions;
using U4.Bot.Builder.Test.Fluent;

namespace $ext_safeprojectname$.Tests.Dialogs.Children
{
    [TestClass]
    public class ChildDialogTests
    {
        [TestMethod]
        public void ChidDialog_happy_path()
        {
            var childDialogFluentTest = new ChildDialogFluentTest()
                .Create()
                .Message(m => {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Please pick a number between one and ten (hint: UnitX).", m[0].Text);
                    return m[0].MakeReply("33");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Please enter a number between one and ten.", m[0].Text);
                    return m[0].MakeReply("10");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, you picked the wrong number. Do you want to try again?", m[0].Text);
                    return m[0].MakeReply("Yes");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Please pick a number between one and ten (hint: UnitX).", m[0].Text);
                    return m[0].MakeReply("1");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Congratulations. You picked the correct number.", m[0].Text);
                });
        }

        [TestMethod]
        public void ChidDialog_pick_number_to_many_attempts()
        {
            var childDialogFluentTest = new ChildDialogFluentTest()
                .Create()
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Please pick a number between one and ten (hint: UnitX).", m[0].Text);
                    return m[0].MakeReply("33");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Please enter a number between one and ten.", m[0].Text);
                    return m[0].MakeReply("33");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Please enter a number between one and ten.", m[0].Text);
                    return m[0].MakeReply("33");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Please enter a number between one and ten.", m[0].Text);
                    return m[0].MakeReply("33");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Too many attempts. I'm ending this conversation.", m[0].Text);
                });
        }

        [TestMethod]
        public void ChidDialog_confirm_retry_too_many_attempts()
        {
            var childDialogFluentTest = new ChildDialogFluentTest()
                .Create()
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Please pick a number between one and ten (hint: UnitX).", m[0].Text);
                    return m[0].MakeReply("5");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, you picked the wrong number. Do you want to try again?", m[0].Text);
                    return m[0].MakeReply("Not a confirmation");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Do you want to enter a new number?", m[0].Text);
                    return m[0].MakeReply("Not a confirmation");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Do you want to enter a new number?", m[0].Text);
                    return m[0].MakeReply("Not a confirmation");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Sorry, I didn't get that. Do you want to enter a new number?", m[0].Text);
                    return m[0].MakeReply("Not a confirmation");
                })
                .Message(m =>
                {
                    Assert.AreEqual(1, m.Count);
                    Assert.AreEqual("Too many attempts. I'm ending this conversation.", m[0].Text);
                });
        }

        private class ChildDialogFluentTest
        {
            public ChildDialogFluentTest()
            {
                BusinessLogicServiceMock = new Mock<IBusinessLogicService>();
                BusinessLogicServiceMock.Setup(b => b.IsLuckyNumber(It.IsAny<int>())).Returns(false);
                BusinessLogicServiceMock.Setup(b => b.IsLuckyNumber(1)).Returns(true);
            }

            public Mock<IBusinessLogicService> BusinessLogicServiceMock { get; private set; }

            public FluentTests<ChildDialogModel, ChildDialog> Create()
            {
                //FluentTest<string, ChildDialog> means that the ChildDialog is of type IDialogContext<string>.
                return new FluentTests<ChildDialogModel, ChildDialog>()
                    .SetContainerMocks(b =>
                    {
                        b.RegisterInstance(BusinessLogicServiceMock.Object).As<IBusinessLogicService>().SingleInstance();
                    })

                    //Constructor parameters to child dialog.
                    //Note that this test states that the dialog is not a child, meaning
                    //that the dialog will resolve with Complete instead of Done. The fluent
                    //test framework requires that you resolve with Complete.
                    .Interactive(BusinessLogicServiceMock.Object, false)
                    .Message("Start");
            }
        }
    }
}
