<template>
    <div id="app-navigation">
        <router-link to="/">Accueil</router-link>
    </div>
</template>

<script scoped>
export default {
    name: 'AppNavigation',
    props: {
        items : {
            type : Array,
            required : false
        }
    }
}
</script>