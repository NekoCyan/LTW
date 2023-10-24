function getAllProducts() {
    return [
        {
            id: 1,
            name: "Men's Shirt",
            imgLink: "/images/p1.png",
            price: 75,
        },
        {
            id: 2,
            name: "Men's Shirt",
            imgLink: "/images/p2.png",
            price: 80,
        },
        {
            id: 3,
            name: "Women's Dress",
            imgLink: "/images/p3.png",
            price: 68,
        },
        {
            id: 4,
            name: "Women's Dress",
            imgLink: "/images/p4.png",
            price: 70,
        },
        {
            id: 5,
            name: "Women's Dress",
            imgLink: "/images/p5.png",
            price: 75,
        },
        {
            id: 6,
            name: "Women's Dress",
            imgLink: "/images/p6.png",
            price: 58,
        },
        {
            id: 7,
            name: "Women's Dress",
            imgLink: "/images/p7.png",
            price: 80,
        },
        {
            id: 8,
            name: "Men's Shirt",
            imgLink: "/images/p8.png",
            price: 65,
        },
        {
            id: 9,
            name: "Men's Shirt",
            imgLink: "/images/p9.png",
            price: 65,
        },
        {
            id: 10,
            name: "Men's Shirt",
            imgLink: "/images/p10.png",
            price: 65,
        },
        {
            id: 11,
            name: "Men's Shirt",
            imgLink: "/images/p11.png",
            price: 65,
        },
        {
            id: 12,
            name: "Women's Dress",
            imgLink: "/images/p12.png",
            price: 65,
        },
    ];
}

function GetAllLocalStorageProducts() {
    /**
     * @type {Array<{id: number, quantity: number}>}
     */
    let products = [];
    if (localStorage.getItem('products')) {
        products = JSON.parse(localStorage.getItem('products'));
    }
    return products;
}

function exportProducts() {
    const allproducts = document.getElementById('allproducts');
    if (!allproducts) return;
    const products = getAllProducts();

    products.forEach((product) => {
        allproducts?.insertAdjacentHTML(`beforeend`, `
            <div class="col-sm-6 col-md-4 col-lg-3">
                <div class="box">
                    <div class="option_container">
                        <div class="options">
                            <a class="option1 addtocart" itemid="${product.id}">
                                Add To Cart
                            </a>
                            <a href="" class="option2">
                                Buy Now
                            </a>
                        </div>
                    </div>
                    <div class="img-box">
                        <img src="${product.imgLink}" alt="" />
                    </div>
                    <div class="detail-box">
                        <h5>
                            ${product.name}
                        </h5>
                        <h6>
                            $${product.price}
                        </h6>
                    </div>
                </div>
            </div>
        `);
    });
}
// exportProducts();

function exportCartProducts() {
    const cartinput = document.getElementsByClassName('cart-input').item(0);
    if (!cartinput) return;
    const AllCarts = GetAllLocalStorageProducts();
    const AllProducts = getAllProducts();

    AllCarts.forEach((cartData) => {
        const productData = AllProducts.find(x => x.id == cartData.id);

        cartinput.insertAdjacentHTML(`beforeend`,`
            <tr>
                <td class="p-4">
                    <div class="media align-items-center">
                        <img src="${productData.imgLink}"
                            class="d-block ui-w-40 ui-bordered mr-4" alt="" />
                        <div class="media-body">
                            <a href="#" class="d-block text-dark">${productData.name}</a>
                        </div>
                    </div>
                </td>
                <td class="text-right align-middle p-4">
                    <select class="form-select" aria-label="Default select example">
                        <option selected>Size</option>
                        <option value="S">S</option>
                        <option value="M">M</option>
                        <option value="L">L</option>
                        <option value="XL">XL</option>
                    </select>
                </td>
                <td class="text-right font-weight-semibold align-middle p-4">$${productData.price}</td>
                <td class="align-middle p-4"><input type="text" class="form-control text-center" value="${cartData.quantity}" /></td>
                <td class="text-right font-weight-semibold align-middle p-4">$${(productData.price * cartData.quantity).toFixed(2)}</td>
                <td class="text-center align-middle px-0"><a href="#"
                    class="shop-tooltip close float-none text-danger" title=""
                    data-original-title="Remove">×</a></td>
            </tr>
        `);
    });
}
exportCartProducts();