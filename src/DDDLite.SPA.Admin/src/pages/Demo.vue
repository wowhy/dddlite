<template>
  <div class="layout-padding">
    <hello></hello>
  </div>
</template>

<script>
export default {
  name: 'demo',
  components: {
    hello: require('@/Hello').default
  }
}
</script>
