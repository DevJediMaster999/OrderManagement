<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="12">
        <v-card class="mt-12">
          <v-card-title>
            <v-row class="flex-grow-1">
              <v-col>
                <h1>Orders</h1>
              </v-col>
              <v-spacer></v-spacer>
              <v-col cols="auto">
                <v-btn icon @click="fetchOrders" color="primary" large>
                  <v-icon large>mdi-refresh</v-icon>
                </v-btn>
              </v-col>
              <v-col cols="auto">
                <v-btn icon @click="openCreateOrderDialog" color="success" large>
                  <v-icon large>mdi-plus</v-icon>
                </v-btn>
              </v-col>
            </v-row>
          </v-card-title>
          <v-divider></v-divider>
          <v-data-table :headers="headers" :items="paginatedOrders" item-value="idOrder"
            class="striped-rows elevation-1" :items-per-page="itemsPerPage" @page-change="onPageChange">
            <template #item.statusName="{ item }">
              <v-chip :color="getStatusColor(item.statusId)" class="status-chip" dark>{{ item.statusName }}</v-chip>
            </template>
            <template #item.totalAmount="{ item }">
              <span>{{ formatCurrency(item.totalAmount) }}</span>
            </template>
            <template #item.actions="{ item }">
              <v-row class="action-row">
                <v-col cols="auto" class="sm-12">
                  <v-select v-if="shouldShowActions(item.statusId)" :items="statuses"
                    v-model="tempStatuses[item.idOrder]" item-text="statusName" item-value="idStatus"
                    label="Select Status" placeholder="Select new status" dense outlined hide-details
                    class="status-select" :disabled="item.statusId === 3 || item.statusId === 4"></v-select>
                </v-col>
                <v-col cols="auto">
                  <v-btn v-if="shouldShowActions(item.statusId)" @click="saveStatus(item.idOrder)" color="success"
                    small>Update</v-btn>
                </v-col>
                <v-spacer></v-spacer>
                <v-col cols="auto">
                  <v-btn v-if="shouldShowActions(item.statusId)" @click="moveToNextStatus(item.idOrder)" color="primary"
                    small>Next Status</v-btn>
                </v-col>
                <v-col cols="auto">
                  <v-btn icon @click="openOrderDetailsDialog(item.idOrder)" color="info">
                    <v-icon>mdi-information-outline</v-icon>
                  </v-btn>
                </v-col>
                <v-col cols="auto" class="sm-12">
                  <v-btn icon @click="confirmDelete(item.idOrder)" color="red">
                    <v-icon>mdi-delete</v-icon>
                  </v-btn>
                </v-col>
              </v-row>
            </template>
          </v-data-table>
          <v-pagination v-model="page" :length="totalPages"></v-pagination>
        </v-card>
        <v-dialog v-model="deleteDialog" max-width="500px">
          <v-card>
            <v-card-title class="headline">Confirm Delete</v-card-title>
            <v-card-text>Are you sure you want to delete this order?</v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="green darken-1" text @click="deleteDialog = false">Cancel</v-btn>
              <v-btn color="green darken-1" text @click="deleteOrder">Delete</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <create-order-dialog v-model="createOrderDialog" @orderCreated="fetchOrders" />
        <order-details-dialog :order-id="selectedOrderId" :dialog-visible="orderDetailsDialogVisible"
          @close="orderDetailsDialogVisible = false" />
        <v-snackbar v-model="snackbar" :timeout="snackbarTimeout">
          {{ snackbarMessage }}
          <v-btn color="pink" text @click="snackbar = false">Close</v-btn>
        </v-snackbar>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import axios from '@/axios';
import CreateOrderDialog from '@/components/orderManagement/CreateOrderDialog.vue';
import OrderDetailsDialog from '@/components/orderManagement/OrderDetailsDialog.vue';

export default {
  components: {
    CreateOrderDialog,
    OrderDetailsDialog
  },
  data() {
    return {
      headers: [
        { text: 'Order ID', value: 'idOrder' },
        { text: 'Shop Name', value: 'shopName' },
        { text: 'Status', value: 'statusName' },
        { text: 'Total Amount', value: 'totalAmount' },
        { text: 'Actions', value: 'actions', sortable: false }
      ],
      tempStatuses: {},
      page: 1,
      itemsPerPage: 10,
      defaultStatus: null,
      deleteDialog: false,
      orderIdToDelete: null,
      createOrderDialog: false,
      orderDetailsDialog: false,
      selectedOrderId: 0,
      snackbar: false,
      snackbarMessage: '',
      snackbarTimeout: 3000,
      orderDetailsDialogVisible: false
    };
  },
  computed: {
    ...mapGetters(['orders', 'statuses']),
    paginatedOrders() {
      const start = (this.page - 1) * this.itemsPerPage;
      const end = this.page * this.itemsPerPage;
      return this.orders.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.orders.length / this.itemsPerPage);
    }
  },
  methods: {
    ...mapActions(['fetchOrders', 'fetchStatuses', 'updateOrderStatus', 'updateOrderToNextStatus', 'removeOrderFromList']),
    getStatusColor(statusId) {
      switch (statusId) {
        case 1:
          return '#4169E1';
        case 2:
          return '#FF8C00';
        case 3:
          return '#32CD32';
        case 4:
          return '#FF0000';
        default:
          return '#808080';
      }
    },
    initializeTempStatuses() {
      this.paginatedOrders.forEach(order => {
        if (!(order.idOrder in this.tempStatuses)) {
          this.$set(this.tempStatuses, order.idOrder, order.statusId);
        }
      });
    },
    saveStatus(idOrder) {
      const updatedStatusId = this.tempStatuses[idOrder];
      this.updateOrderStatus({ id: idOrder, statusId: updatedStatusId })
        .then(() => {
          const order = this.orders.find(o => o.idOrder === idOrder);
          if (order) {
            order.statusId = updatedStatusId;
          }
        })
        .catch(error => {
          console.error('Error updating status for order:', idOrder, error);
        });
    },
    moveToNextStatus(idOrder) {
      this.updateOrderToNextStatus(idOrder)
        .then(() => {
          const order = this.orders.find(o => o.idOrder === idOrder);
          if (order) {
            order.statusId += 1;
          }
        })
        .catch(error => {
          console.error('Error moving to next status for order:', idOrder, error);
        });
    },
    confirmDelete(idOrder) {
      this.orderIdToDelete = idOrder;
      this.deleteDialog = true;
    },
    deleteOrder() {
      const idOrder = this.orderIdToDelete;
      axios.patch(`orders/${idOrder}/disable`)
        .then(() => {
          this.removeOrderFromList(idOrder);
          this.snackbarMessage = `Order ${idOrder} deleted successfully.`;
          this.snackbar = true;
        })
        .catch(error => {
          console.error('Error deleting order:', idOrder, error);
          this.snackbarMessage = `Error deleting order ${idOrder}.`;
          this.snackbar = true;
        })
        .finally(() => {
          this.deleteDialog = false;
        });
    },
    formatCurrency(amount) {
      return `$${parseFloat(amount).toFixed(2)}`;
    },
    openCreateOrderDialog() {
      this.createOrderDialog = true;
    },
    openOrderDetailsDialog(orderId) {
      this.selectedOrderId = 0;
      this.$nextTick(() => {
        this.selectedOrderId = orderId;
        this.orderDetailsDialogVisible = true;
      });
    },
    onPageChange(newPage) {
      this.page = newPage;
      this.initializeTempStatuses();
    },
    shouldShowActions(statusId) {
      return statusId !== 3 && statusId !== 4;
    }
  },
  watch: {
    orders: {
      handler() {
        this.initializeTempStatuses();
      },
      deep: true,
      immediate: true
    }
  },
  mounted() {
    this.fetchOrders().then(() => {
      this.initializeTempStatuses();
    });
    this.fetchStatuses();
  }
};
</script>

<style>
.v-card-title {
  padding: 20px;
  background-color: #f5f5f5;
  border-bottom: 1px solid #e0e0e0;
}

.v-chip {
  font-weight: bold;
}

.v-btn {
  text-transform: none;
}

.v-data-table-header th {
  background-color: #f5f5f5;
}

.status-select {
  width: 200px;
  margin: 10px 0;
  background: #FFFFFF;
}

.status-chip {
  width: 100px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.action-row {
  align-items: center;
  margin-top: 3px;
  margin-bottom: 3px;
}

.striped-rows .v-data-table__wrapper tbody tr:nth-child(odd) {
  background-color: #f5f5f5;
}

.striped-rows .v-data-table__wrapper tbody tr:nth-child(even) {
  background-color: #ffffff;
}
</style>
