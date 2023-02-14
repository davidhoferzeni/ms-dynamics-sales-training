
function mainAccountFormOnLoad(executionContext) {
    const formContext = executionContext.getFormContext();
    let attributeName = formContext.getAttribute("name").getValue();
    let attributeIndexNumber = formContext.getAttribute("new_accountindex").getValue();
    alert(`The current account index of account ${attributeName} is: ${attributeIndexNumber}.`);
}