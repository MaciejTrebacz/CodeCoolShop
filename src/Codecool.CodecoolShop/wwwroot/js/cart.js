function testFunc(event) {
    let element = event.currentTarget;
    let newQuantity = element.value;

    element.defaultValue = newQuantity;

    let productId = element.getAttribute("data-id");
    let subtotalElement = document.getElementById("subtotal-" + productId);
    let price = parseFloat(document.getElementById("price-" + productId).innerText);
    let oldSubtotal = parseFloat(subtotalElement.innerText);

    let newSubtotal = (price * newQuantity).toFixed(2);

    subtotalElement.innerText = newSubtotal;

    let totalPriceElement = document.getElementById("totalPrice");
    let totalPrice = parseFloat(totalPriceElement.innerText);

    totalPrice = totalPrice - oldSubtotal;
    totalPrice = (totalPrice + parseFloat(newSubtotal)).toFixed(2);

    totalPriceElement.innerText = totalPrice;
}