<!-- Site footer -->
<script setup>
    import VersionInfo from './VersionInfo.vue'
    import SiteFriends from "./SiteFriends.vue";

</script>

<template>
    <footer class="footer">
        <div class="horizontal-flex-center">
            
            <div class="footer-info-column">
                <div>
                    furtails-logo
                </div>
                <div>
                    
                </div>
            </div>

            <div class="footer-useful-column">
                <div class="footer-column-header">
                    Полезное
                </div>
                <div>
                    Useful
                </div>
            </div>
            
            <div class="footer-developers-column">
                <div class="footer-column-header">
                    Разработчики
                </div>
                <div>
                    <div class="footer-block">
                        Изначальный дизайн и php-программирование - Şeti
                    </div>

                    <div class="footer-block">
                        Редизайн, программирование - <a href="https://fossa.life" title="Блог Белой Фоссы">Белая Фосса</a>
                    </div>

                    <div class="footer-block">
                        Идея сайта и перевод текстов - <a href="mailto:redgerra@rambler.ru">Redgerra</a>
                    </div>
                </div>
            </div>
            
            <div class="footer-friends-column">
                <div class="footer-column-header">
                    Друзья сайта
                </div>
                <div>
                    <SiteFriends />
                </div>
            </div>
            
            <div class="footer-partners-column">
                <div class="footer-column-header">
                    Реклама
                </div>
                <div>
                    Useful
                </div>
            </div>
        </div>

        <!-- Version info string -->
        <VersionInfo />
    </footer>
</template>