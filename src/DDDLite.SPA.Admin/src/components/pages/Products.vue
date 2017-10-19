<template>
  <div class="layout-padding">
  </div>
</template>

<script>
import productService from '~/services/product'
export default {
  data() {
    return {
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
