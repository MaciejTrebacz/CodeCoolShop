function testFunc(event) {
    let element = event.currentTarget;
    let productId = element.getAttribute("data-id");
    let newQuantity = element.value;

    changeQuantityServerside(parseInt(productId), parseInt(newQuantity));

    element.defaultValue = newQuantity;

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

function changeQuantityServerside(productId, quantity) {
    fetch("/api/CartApi/AdjustCartQuantity", {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            productId: productId,
            quantity: quantity
        })
    });

}

function removeItem(event) {
    var productId = event.currentTarget.getAttribute("data-id");

    let subtotalElement = document.getElementById("subtotal-" + productId);
    let oldSubtotal = parseFloat(subtotalElement.innerText);

    let totalPriceElement = document.getElementById("totalPrice");
    let totalPrice = parseFloat(totalPriceElement.innerText);

    totalPrice = (totalPrice - oldSubtotal).toFixed(2);

    totalPriceElement.innerText = totalPrice;

    document.getElementById("row-" + productId).remove();

    fetch("/api/CartApi/RemoveFromCart", {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            productId: productId
        })
    });
}