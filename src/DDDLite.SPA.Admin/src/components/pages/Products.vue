<template>
  <div class="layout-padding">
    <q-data-table :config="config" :columns="columns" :data="data" @refresh="refresh">
      <template slot="col-detail" slot-scope="cell">
        <span class="light-paragraph">{{cell.data.memo}}</span>
      </template>
    </q-data-table>
  </div>
</template>

<script>
import productService from '~/services/product'
export default {
  data() {
    return {
      config: {
        rowHeight: '50px',
        refresh: true,
        title: '产品列表',
        messages: {
          noData: '没有相关数据。',
          noDataAfterFiltering: '没有相关数据。'
        },
        pagination: {
          rowsPerPage: 15,
          options: [5, 10, 15, 30, 50, 500]
        }
      },
      columns: [
        { label: '代码', field: 'code', sort: true },
        { label: '名称', field: 'name', sort: true },
        { label: '备注', field: 'detail', sort: false }
      ],
      data: [],
      total: 0
    }
  },
  methods: {
    async refresh(done) {
      let result = await productService.find(1, 10)
      this.data = result.value
      if (done) {
        done()
      }
    }
  },
  mounted() {
    this.refresh()
  }
}
</script>

<style>

</style>
