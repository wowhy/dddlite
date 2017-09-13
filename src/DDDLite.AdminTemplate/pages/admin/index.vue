<template>
  <div>
    {{message}}
  </div>
</template>

<script>
export default {
  data() {
    return {
      message: 'Hello, World!'
    }
  },
  head() {
    return {
      title: '后台管理'
    }
  }
}
</script>
