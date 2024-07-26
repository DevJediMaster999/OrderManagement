<template>
    <v-dialog v-model="internalDialog" max-width="600px">
        <v-card>
            <v-card-title class="headline d-flex justify-space-between align-center">
                <span>Order Details</span>
                <v-btn icon @click="closeDialog">
                    <v-icon>mdi-close</v-icon>
                </v-btn>
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text>
                <v-row class="mt-3">
                    <v-col cols="12">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-icon color="blue">mdi-storefront</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                <v-list-item-title class="font-weight-bold">Shop Name:</v-list-item-title>
                                <v-list-item-subtitle>{{ orderDetails.nameShop || 'N/A' }}</v-list-item-subtitle>
                            </v-list-item-content>
                        </v-list-item>
                    </v-col>
                    <v-col cols="12">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-icon :color="getStatusColor(orderDetails.statusName)">mdi-information</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                <v-list-item-title class="font-weight-bold">Status:</v-list-item-title>
                                <v-list-item-subtitle>{{ orderDetails.statusName || 'N/A' }}</v-list-item-subtitle>
                            </v-list-item-content>
                        </v-list-item>
                    </v-col>
                    <v-col cols="12">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-icon color="purple">mdi-calendar</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                <v-list-item-title class="font-weight-bold">Date Order:</v-list-item-title>
                                <v-list-item-subtitle>{{ orderDetails.dateOrder ? formatDate(orderDetails.dateOrder) :
                                    'N/A' }}</v-list-item-subtitle>
                            </v-list-item-content>
                        </v-list-item>
                    </v-col>
                    <v-col cols="12">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-icon color="green">mdi-currency-usd</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                <v-list-item-title class="font-weight-bold">Total Amount:</v-list-item-title>
                                <v-list-item-subtitle>{{ formatCurrency(orderDetails.totalAmount)
                                    }}</v-list-item-subtitle>
                            </v-list-item-content>
                        </v-list-item>
                    </v-col>
                    <v-col cols="12">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-icon color="orange">mdi-package-variant</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                <v-list-item-title class="font-weight-bold">Products:</v-list-item-title>
                                <v-list-item-subtitle>
                                    <v-list dense class="products-list">
                                        <v-list-item v-for="product in orderDetails.products.$values"
                                            :key="product.productId">
                                            <v-list-item-icon>
                                                <v-icon color="teal">mdi-cube-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>
                                                <v-row>
                                                    <v-col class="product-column">
                                                        {{ product.productName }}
                                                    </v-col>
                                                    <v-col class="product-column" cols="auto">
                                                        {{ formatCurrency(product.price) }}
                                                    </v-col>
                                                    <v-col class="product-column" cols="auto">
                                                        (Quantity: {{ product.quantity }})
                                                    </v-col>
                                                </v-row>
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-list-item-subtitle>
                            </v-list-item-content>
                        </v-list-item>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" text @click="closeDialog">Close</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import axios from '@/axios';

export default {
    props: {
        orderId: {
            type: Number,
            required: true
        },
        dialogVisible: {
            type: Boolean,
            required: true
        }
    },
    data() {
        return {
            internalDialog: false,
            orderDetails: {
                nameShop: '',
                statusName: '',
                dateOrder: '',
                totalAmount: 0,
                products: []
            }
        };
    },
    watch: {
        dialogVisible(newVal) {
            this.internalDialog = newVal;
            if (newVal) {
                this.fetchOrderDetails();
            }
        },
        internalDialog(newVal) {
            if (!newVal) {
                this.$emit('close');
            }
        }
    },
    methods: {
        async fetchOrderDetails() {
            if (this.orderId) {
                try {
                    const response = await axios.get(`orders/${this.orderId}`);
                    this.orderDetails = response.data;
                } catch (error) {
                    console.error('Error fetching order details:', error);
                }
            }
        },
        formatDate(date) {
            const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit', second: '2-digit' };
            return new Date(date).toLocaleDateString('en-US', options);
        },
        formatCurrency(amount) {
            return `$${parseFloat(amount).toFixed(2)}`;
        },
        closeDialog() {
            this.internalDialog = false;
        },
        getStatusColor(statusName) {
            switch (statusName) {
                case 'New':
                    return '#4169E1';
                case 'Processing':
                    return '#FF8C00';
                case 'Completed':
                    return '#32CD32';
                case 'Canceled':
                    return '#FF0000';
                default:
                    return '#808080';
            }
        }
    }
};
</script>

<style scoped>
.v-card-title {
    background-color: #f5f5f5;
    border-bottom: 1px solid #e0e0e0;
}

.v-card-actions {
    background-color: #f5f5f5;
    border-top: 1px solid #e0e0e0;
}

.products-list {
    max-height: 200px;
    overflow-y: auto;
}

.products-list .v-list-item {
    border-bottom: 1px solid #e0e0e0;
}

.products-list .v-list-item:last-child {
    border-bottom: none;
}

.product-column {
    padding: 12px;
}
</style>
