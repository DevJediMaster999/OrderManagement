<template>
    <v-dialog v-model="internalDialog" max-width="600px" persistent>
        <v-card>
            <v-card-title class="headline">Create New Order</v-card-title>
            <v-card-text>
                <v-form>
                    <v-row>
                        <v-col cols="12">
                            <v-combobox :items="shops.$values" v-model="selectedShop" item-text="shopName"
                                item-value="idShop" label="Select Shop" @change="fetchProducts" outlined
                                :loading="loadingShops" :search-input.sync="shopSearch"
                                @update:search-input="fetchShops" dense hide-details class="mb-3"
                                prepend-inner-icon="mdi-store"></v-combobox>
                        </v-col>
                        <v-col cols="12">
                            <v-select :items="productsWithPrices" v-model="selectedProducts"
                                item-text="productNameWithPrice" item-value="idProduct" label="Select Products" multiple
                                chips hide-details deletable-chips outlined :loading="loadingProducts"
                                :disabled="!selectedShop" dense class="mb-3 small-chips"
                                prepend-inner-icon="mdi-cart"></v-select>
                        </v-col>
                    </v-row>
                </v-form>
                <v-row>
                    <v-col cols="12" class="text-left">
                        <h3>Total: {{ totalAmount }} USD</h3>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="red darken-1" text @click="closeDialog">Cancel</v-btn>
                <v-btn color="green darken-1" text @click="createOrder"
                    :disabled="!selectedShop || selectedProducts.length === 0">Create</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import axios from '@/axios';

export default {
    props: {
        value: {
            type: Boolean,
            required: true
        }
    },
    data() {
        return {
            internalDialog: this.value,
            selectedShop: null,
            selectedProducts: [],
            shops: [],
            products: [],
            loadingShops: false,
            loadingProducts: false,
            shopSearch: ''
        };
    },
    computed: {
        productsWithPrices() {
            if (Array.isArray(this.products)) {
                return this.products.map(product => ({
                    ...product,
                    productNameWithPrice: `${product.productName} - $${product.price}`
                }));
            }
            return [];
        },
        totalAmount() {
            return this.selectedProducts.reduce((total, productId) => {
                const product = this.products.find(p => p.idProduct === productId);
                return total + (product ? product.price : 0);
            }, 0).toFixed(2);
        }
    },
    watch: {
        value(val) {
            this.internalDialog = val;
            if (val) {
                this.fetchShops();
            }
        },
        internalDialog(val) {
            this.$emit('input', val);
        }
    },
    methods: {
        fetchShops() {
            this.loadingShops = true;
            axios.get("shops")
                .then(response => {
                    this.shops = response.data;
                })
                .catch(error => {
                    console.error('Error fetching shops:', error);
                })
                .finally(() => {
                    this.loadingShops = false;
                });
        },
        fetchProducts() {
            if (this.selectedShop) {
                this.loadingProducts = true;
                axios.get(`products/by-shop/${this.selectedShop.idShop}`)
                    .then(response => {
                        if (response.data && Array.isArray(response.data.$values)) {
                            this.products = response.data.$values;
                        } else {
                            this.products = [];
                        }
                    })
                    .catch(error => {
                        console.error('Error fetching products:', error);
                    })
                    .finally(() => {
                        this.loadingProducts = false;
                    });
            }
        },
        createOrder() {
            const totalPrice = this.selectedProducts.reduce((total, productId) => {
                const product = this.products.find(p => p.idProduct === productId);
                return total + (product ? product.price : 0);
            }, 0).toFixed(2);
            const orderData = {
                shopId: this.selectedShop.idShop,
                totalAmount: totalPrice,
                orderProducts: this.selectedProducts.map(productId => ({
                    productId: productId,
                    quantity: 1
                }))
            };

            axios.post("orders", orderData)
                .then(() => {
                    this.$emit('orderCreated');
                    this.closeDialog();
                })
                .catch(error => {
                    console.error('Error creating order:', error);
                });
        },
        closeDialog() {
            this.internalDialog = false;
            this.selectedShop = null;
            this.selectedProducts = [];
            this.products = [];
        }
    }
};
</script>

<style>
.v-card-title {
    background-color: #f5f5f5;
    border-bottom: 1px solid #e0e0e0;
}

.v-card-actions {
    background-color: #f5f5f5;
    border-top: 1px solid #e0e0e0;
}

.v-combobox,
.v-select {
    margin-bottom: 8px;
}

.v-btn {
    text-transform: none;
}

.small-chips .v-chip {
    height: 24px;
    font-size: 12px;
    line-height: 24px;
}

.small-chips .v-chip__close {
    font-size: 12px;
    height: 16px;
    width: 16px;
    margin-top: 4px;
}
</style>