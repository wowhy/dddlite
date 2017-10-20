<template>
  <div v-loading="loading">
    <h3>产品列表</h3>
    <el-row type="flex">
      <el-form :inline="true" :model="searchForm">
        <el-form-item>
          <el-input prefix-icon="el-icon-search" v-model="searchForm.query" placeholder="代码|名称"></el-input>
        </el-form-item>
        <input class="hidden" type="text">
        <el-form-item>
          <el-button icon="el-icon-search" type="primary" @click="search">查询</el-button>
        </el-form-item>
        <el-form-item>
          <el-button icon="fa fa-lg fa-refresh" type="primary" @click="refresh">&nbsp;&nbsp;刷新</el-button>
        </el-form-item>
        <el-form-item>
          <el-button icon="el-icon-plus" type="success" @click="goAdd">新增</el-button>
        </el-form-item>
      </el-form>
    </el-row>
    <el-row type="flex">
      <el-table class="datatable full-width"
        border highlight-current-row
        :data="data">
        <el-table-column prop="code" label="代码" width="120"></el-table-column>
        <el-table-column prop="name" label="名称" width="120"></el-table-column>
        <el-table-column prop="detail.memo" label="备注"></el-table-column>

        <el-table-column fixed="right" label="操作" width="150">
          <template slot-scope="scope">
            <el-button type="text" @click="goEdit(scope.row)">编辑</el-button>
            <el-button type="text" @click="goDetail(scope.row)">查看</el-button>
            <el-button type="text" class="danger" @click="remove(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-row>

    <el-row type="flex" justify="center">
      <el-pagination layout="total, prev, pager, next, jumper" :total="total" :current-page.sync="page" @current-change="pagination"></el-pagination>
    </el-row>
  </div>
</template>

<script>
import service from '~/services/product'
import pageList from '~/mixins/page-list'
export default {
  mixins: [pageList],
  data() {
    return {
      loading: false,
      data: [],
      total: 0,
      pageSize: 10,
      page: 1,
      searchForm: {
        query: ''
      },
      config: {
        filter: null,
        orderBy: null,
        includes: []
      }
    }
  },
  methods: {
    async refresh() {
      this.loading = true
      try {
        const { filter, orderBy, includes } = this.config
        let result = await service.find(this.page, this.pageSize, filter, orderBy, includes)
        this.data = result.value
        this.total = result.count
      } catch (ex) {
      }

      this.loading = false
    },
    async pagination(val) {
      this.page = val
      await this.refresh()
    },
    async search() {
      this.config.filter = `name contains '${this.searchForm.query}' or code contains '${this.searchForm.query}'`
      if (this.page === 1) {
        await this.refresh()
      } else {
        this.page = 1
      }
    },
    goEdit(row) {
      this.go(`/products/edit?id=${row.id}`)
    },
    goDetail(row) {
      this.go(`/products/detail/${row.id}`)
    },
    async remove(row) {
      try {
        await this.$confirm('此操作将永久删除该数据, 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        })

        this.loading = true
        await service.remove(row.id, row.rowVersion)
        await this.refresh()
      } catch (ex) {
        // nothing
      }

      this.loading = false
    },
    goAdd() {}
  },
  mounted() {
    this.refresh()
  }
}
</script>

<style lang="stylus" scoped>

</style>
