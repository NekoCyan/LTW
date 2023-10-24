function FormatCurrency(number) {
    // return $ (dollar currency with fixed 2 numbers)
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(number);
}
function GetTotalPrice() {
    const totalprice = (value) => {
        const TotalPrice = document.getElementsByClassName('totalprice');
        for (let i = 0; TotalPrice.item(i) != null; i++) {
            TotalPrice.item(i).innerHTML = value;
        }
    }
    const AllProducts = getAllProducts();
    let products = GetAllLocalStorageProducts();
    if (!products[0]) totalprice(FormatCurrency(0));
    else totalprice(FormatCurrency(
        products.reduce((pre, cur) => {
            return pre + cur.quantity * AllProducts.find(x => x.id === cur.id).price;
        }, 0)
    ));
}
GetTotalPrice();

function AddToCartEventListeners() {
    const addtocardbtns = document.getElementsByClassName('addtocart');
    for (let i = 0; i < addtocardbtns.length; i++) {
        const addtocardbtn = addtocardbtns.item(i);
        addtocardbtn.addEventListener('click', () => {
            const productId = parseInt(addtocardbtn.getAttribute('itemid'));
            AddProduct(productId);
            const AllProducts = getAllProducts();
            const productData = AllProducts.find(x => x.id === productId);
            toastr.success(`${productData.name} with $${productData.price}`, `Added to Cart`);
            GetTotalPrice();
        });
    }
}
AddToCartEventListeners();

function AddProduct(productId, specificQuantity = 1) {
    let products = GetAllLocalStorageProducts();
    const exists = products.find(x => x.id === productId);
    if (exists) {
        if (specificQuantity != 1) {
            exists.quantity = specificQuantity;
        } else {
            exists.quantity++;
        }
    } else {
        products.push({ id: productId, quantity: 1 });
    }
    localStorage.setItem('products', JSON.stringify(products));
    GetTotalPrice();
}

function RemoveProduct(productId, specificQuantity = 1) {
    let products = GetAllLocalStorageProducts();
    const exists = products.find(x => x.id === productId);
    if (!exists) return;

    if (specificQuantity != 1) {
        if (exists.quantity == 1) {
            products = products.filter(x => x.id !== productId);
        } else {
            exists.quantity--;
        }
    } else {
        exists.quantity -= specificQuantity;

        if (exists.quantity <= 0) {
            products = products.filter(x => x.id !== productId);
        }
    }

    localStorage.setItem('products', JSON.stringify(products));
    GetTotalPrice();
}