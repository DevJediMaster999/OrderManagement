import Vue from 'vue';
import Vuex from 'vuex';
import axios from '@/axios';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    orders: [],
    statuses: []
  },
  mutations: {
    SET_ORDERS(state, orders) {
      state.orders = Array.isArray(orders) ? orders : [];
    },
    SET_STATUSES(state, statuses) {
      state.statuses = statuses;
    },
    REMOVE_ORDER(state, idOrder) {
      state.orders = state.orders.filter(order => order.idOrder !== idOrder);
    }
  },
  actions: {
    async fetchOrders({ commit }) {
      try {
        const response = await axios.get('orders');
        commit('SET_ORDERS', response.data.$values);
      } catch (error) {
        console.error('Error fetching orders:', error);
      }
    },
    async fetchStatuses({ commit }) {
      try {
        const response = await axios.get('orders/status-list');
        commit('SET_STATUSES', response.data.$values);
      } catch (error) {
        console.error('Error fetching statuses:', error);
      }
    },
    async updateOrderToNextStatus({ dispatch }, id) {
      try {
        await axios.patch(`orders/${id}/next-status`);
        dispatch('fetchOrders');
      } catch (error) {
        console.error('Error updating order to next status:', error);
      }
    },
    async updateOrderStatus({ dispatch }, { id, statusId }) {
      try {
        await axios.patch(`orders/${id}/status`, { idStatus: statusId });
        dispatch('fetchOrders');
      } catch (error) {
        console.error('Error updating order status:', error);
      }
    },
    async removeOrderFromList({ commit }, idOrder) {
      commit('REMOVE_ORDER', idOrder);
    }
  },
  getters: {
    orders: state => state.orders,
    statuses: state => state.statuses
  }
});
